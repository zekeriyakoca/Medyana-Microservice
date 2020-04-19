using Dtos.Common;
using Dtos.Enums;
using Dtos.Equipment;
using Medyana.Inventory.Domain.Entities;
using Medyana.Inventory.Domain.Interface;
using Medyana.Dtos.Clinic;
using Medyana.Dtos.Equipment;
using Medyana.Inventory.API.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Infrastructure;
using Grpc.Core;
using Newtonsoft.Json;
using Medyana.Inventory.API.Mappers;

namespace Medyana.Inventory.API.Services
{
  public class EquipmentAppService : EquipmentService.EquipmentServiceBase
  {
    public IEquipmentRepository equipmentRepository { get; }
    public IClinicRepository clinicRepository { get; }

    public EquipmentAppService(IEquipmentRepository equipmentRepository, IClinicRepository clinicRepository)
    {
      this.equipmentRepository = equipmentRepository;
      this.clinicRepository = clinicRepository;
    }

    public async override Task<CommonResponseMessage> GetEquipment(CommonRequestMessage request, ServerCallContext context)
    {
      var equipmentId = JsonConvert.DeserializeObject<int>(request.Content);
      var equipment = (await equipmentRepository.Get(equipmentId))?.ToEquipmentDetailDto();
      var result = new CommonResponseMessage { Content = JsonConvert.SerializeObject(equipment), Message = "Success", Status = true };
      return result;
    }

    public async override Task<CommonResponseMessage> DeleteEquipment(CommonRequestMessage request, ServerCallContext context)
    {
      var equipmentId = JsonConvert.DeserializeObject<int>(request.Content);

      var equipmentToDelete = await equipmentRepository.Get(equipmentId);
      if (equipmentToDelete == null)
        return new CommonResponseMessage { Content = "", Message = "Fail", Status = false };

      equipmentRepository.Remove(equipmentToDelete);

      await equipmentRepository.SaveChangesAsync();

      return new CommonResponseMessage { Content = "", Message = "Success", Status = true };
    }

    public async override Task<CommonResponseMessage> InsertEquipment(CommonRequestMessage request, ServerCallContext context)
    {
      var equipmentToInsert = JsonConvert.DeserializeObject<EquipmentInsertDto>(request.Content);
      if (!await clinicRepository.Any(equipmentToInsert.ClinicId))
      {
        return new CommonResponseMessage { Content = "", Message = $"There is no clinic with id : {equipmentToInsert.ClinicId}", Status = false };
      }

      var equipment = equipmentToInsert.ToEquipment();
      equipmentRepository.Add(equipment);

      await equipmentRepository.SaveChangesAsync();

      var result = equipment.ToEquipmentDetailDto();

      return new CommonResponseMessage { Content = JsonConvert.SerializeObject(result), Message = "Success", Status = true };
    }

    public async override Task<CommonResponseMessage> UpdateEquipment(CommonRequestMessage request, ServerCallContext context)
    {
      var dto = JsonConvert.DeserializeObject<EquipmentUpdateDto>(request.Content);
      if (!await clinicRepository.Any(dto.ClinicId))
      {
        return new CommonResponseMessage { Content = "", Message = $"There is no clinic with id : {dto.ClinicId}", Status = false };
      }

      var equipment = await equipmentRepository.Get(dto.Id);
      if (equipment == null)
        return new CommonResponseMessage { Content = "", Message = "Unable to find equipment to update", Status = false };

      equipment.Parse(dto);
      await equipmentRepository.SaveChangesAsync();

      var result = equipment.ToEquipmentDetailDto();

      return new CommonResponseMessage { Content = JsonConvert.SerializeObject(result), Message = "Success", Status = true };
    }

    public async override Task<CommonResponseMessage> GetEquipments(CommonRequestMessage request, ServerCallContext context)
    {
      var dto = JsonConvert.DeserializeObject<EquipmentPaginationRequestDto>(request.Content);

      var query = await equipmentRepository.GetAllIncludeAll();

      if (dto.ClinicId > 0)
        query = query.Where(c => c.ClinicId == dto.ClinicId);

      var totalItem = query.Count();

      query = SortAndFilterEquipments(dto, query);

      query = query.Skip(dto.Page * dto.PageItemCount)
                   .Take(dto.PageItemCount);

      var equipments = query?.ToEquipmentItemDtoList();

      var result =  new Paginatedlist<EquipmentItemDto>(equipments, dto.Page, totalItem, dto.PageItemCount);
     
      return new CommonResponseMessage { Content = JsonConvert.SerializeObject(result), Message = "Success", Status = true };
    }


    #region Private Methods 

    private static IEnumerable<Equipment> SortAndFilterEquipments(EquipmentPaginationRequestDto dto, IEnumerable<Equipment> query)
    {
      var property = typeof(Equipment).GetProperties()
                        .Where(p => p.CanWrite && p.Name.ToLower() == dto.Column?.ToLower())
                        .SingleOrDefault();

      switch (dto.Type)
      {
        case PaginationType.Sorting:
          query = dto.IsAscending
                      ? query.OrderBy(c => property.GetValue(c))
                      : query.OrderByDescending(c => property.GetValue(c));
          break;
        case PaginationType.Searching:
          query = query.Where(c => c.Name.ToLower().Contains(dto.SearchText.ToLower()));
          break;
        default:
          break;
      }

      return query;
    }

    #endregion
  }
}
