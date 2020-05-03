using Dtos.Common;
using Dtos.Enums;
using Dtos.Equipment;
using Grpc.Net.Client;
using Medyana.Dtos.Clinic;
using Medyana.Dtos.Equipment;
using Medyana.Inventory.API.Services;
using Medyana.Web.Bff.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Infrastructure;
using static Medyana.Inventory.API.Services.EquipmentService;

namespace Medyana.Web.Bff.Service
{
  public class EquipmentService : BaseGrpcService<EquipmentServiceClient>, IEquipmentService
  {
    private readonly AppSettings appSettings;

    public EquipmentService(IOptionsSnapshot<AppSettings> settings) : base(settings)
    {
      this.appSettings = settings.Value;
    }

    public async Task<EquipmentDetailDto> GetEquipment(int equipmentId)
    {
      var response = await client.GetEquipmentAsync(new CommonRequestMessage { Content = JsonConvert.SerializeObject(equipmentId) });
      var result = JsonConvert.DeserializeObject<EquipmentDetailDto>(response.Content);
      return result;
    }

    public async Task<Paginatedlist<EquipmentItemDto>> GetAllEquipments(EquipmentPaginationRequestDto dto)
    {
      var response = await client.GetEquipmentsAsync(new CommonRequestMessage { Content = JsonConvert.SerializeObject(dto) });
      var result = JsonConvert.DeserializeObject<Paginatedlist<EquipmentItemDto>>(response.Content);
      return result;
    }

    public async Task<EquipmentDetailDto> InsertEquipment(EquipmentInsertDto equipmentToInsert)
    {
      var response = await client.InsertEquipmentAsync(new CommonRequestMessage { Content = JsonConvert.SerializeObject(equipmentToInsert) });
      var result = JsonConvert.DeserializeObject<EquipmentDetailDto>(response.Content);
      return result;
    }

    public async Task<EquipmentDetailDto> UpdateEquipment(EquipmentUpdateDto dto)
    {
      var response = await client.UpdateEquipmentAsync(new CommonRequestMessage { Content = JsonConvert.SerializeObject(dto) });
      var result = JsonConvert.DeserializeObject<EquipmentDetailDto>(response.Content);
      return result;
    }

    public async Task<bool> DeleteEquipment(int equipmentId)
    {
      var response = await client.DeleteEquipmentAsync(new CommonRequestMessage { Content = JsonConvert.SerializeObject(equipmentId) });
      return response.Status;
    }
  }
}
