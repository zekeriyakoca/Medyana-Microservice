using Dtos.Common;
using Grpc.Core;
using Medyana.Inventory.Domain.Entities;
using Medyana.Inventory.Domain.Interface;
using Medyana.Dtos.Clinic;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dtos.Enums;
using Medyana.Inventory.API.Mappers;

namespace Medyana.Inventory.API.Services
{
  public class ClinicAppService : ClinicService.ClinicServiceBase
  {
    public ClinicAppService(IClinicRepository clinicRepository) : base()
    {
      this.clinicRepository = clinicRepository;
    }

    public IClinicRepository clinicRepository { get; }

    public override async Task<GetClinicResponseMessage> GetClinic(GetClinicReqeustMessage request, ServerCallContext context)
    {
      var clinic = await clinicRepository.Get(request.ClinicId);
      return new GetClinicResponseMessage { Clinic = clinic?.ToClinicDetailMessage() };
    }

    public async override Task<DeleteClinicResponseMessage> DeleteClinic(DeleteClinicReqeustMessage request, ServerCallContext context)
    {
      var resp = new DeleteClinicResponseMessage();

      var clinicToDelete = await clinicRepository.Get(request.ClinicId);
      if (clinicToDelete == null)
        resp.Result = false;

      clinicRepository.Remove(clinicToDelete);
      var result = await clinicRepository.SaveChangesAsync();

      if (result < 1)
        resp.Result = false;
      else
        resp.Result = true;
      return resp;
    }

    public async override Task<InsertClinicResponseMessage> InsertClinic(InsertClinicReqeustMessage request, ServerCallContext context)
    {
      var clinic = request.ToClinic();
      clinicRepository.Add(clinic);

      await clinicRepository.SaveChangesAsync();

      return new InsertClinicResponseMessage { ClinicCreated = clinic.ToClinicItemMessage() };
    }

    public async override Task<UpdateClinicResponseMessage> UpdateClinic(UpdateClinicReqeustMessage request, ServerCallContext context)
    {
      var clinic = await clinicRepository.Get(request.ClinicId);
      if (clinic == null)
        throw new Exception("Unable to find clinic to update");

      clinic.Parse(request);
      await clinicRepository.SaveChangesAsync();

      return new UpdateClinicResponseMessage { Result = true };
    }

    public async override Task<CommonResponseMessage> GetClinics(CommonRequestMessage request, ServerCallContext context)
    {
      var result = new CommonResponseMessage();

      var dto = JsonConvert.DeserializeObject<PaginationRequestDto>(request.Content);
      var query = await clinicRepository.GetAll();
      var totalItem = query.Count();
      var property = typeof(Clinic).GetProperties().Where(p => p.CanWrite && p.Name.ToLower() == dto.Column?.ToLower()).SingleOrDefault();

      switch (dto.Type)
      {
        case PaginationType.Sorting:
          query = dto.IsAscending ? query.OrderBy(c => property.GetValue(c)) : query.OrderByDescending(c => property.GetValue(c));
          break;
        case PaginationType.Searching:
          query = query.Where(c => c.Name != null && c.Name.ToLower().Contains(dto.SearchText?.ToLower()));
          break;
        default:
          break;
      }
      query = query.Skip(dto.Page * dto.PageItemCount).Take(dto.PageItemCount);

      var clinics = query?.ToClinicItemDtoList();

      var paginatedList = new Paginatedlist<ClinicItemDto>(clinics, dto.Page, totalItem, dto.PageItemCount);

      return new CommonResponseMessage { Content = JsonConvert.SerializeObject(paginatedList),Message = "Success", Status = true };
       
    }
  }
}
