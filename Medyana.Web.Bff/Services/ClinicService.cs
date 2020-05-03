using Dtos.Common;
using Dtos.Enums;
using Grpc.Net.Client;
using Medyana.Dtos.Clinic;
using Medyana.Inventory.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medyana.Web.Bff.Mappers;
using static Medyana.Inventory.API.Services.ClinicService;
using Medyana.Web.Bff;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Medyana.Web.Bff.Services;

namespace Medyana.Web.Bff.Service
{
  public class ClinicService : BaseGrpcService<ClinicServiceClient>, IClinicService
  {
    private readonly AppSettings appSettings;
    public ClinicService(IOptionsSnapshot<AppSettings> settings) : base(settings)
    {
      this.appSettings = settings.Value;
    }

    public async Task<bool> DeleteClinic(int clinicId)
    {
      var result = await client.DeleteClinicAsync(new DeleteClinicReqeustMessage { ClinicId = clinicId });
      return result.Result;
    }

    //public async Task<Paginatedlist<ClinicItemDto>> GetAllClinics(PaginationRequestDto dto)
    //{
    //  var result = await client.GetClinicsAsync(dto.ToGetClinicsRequestMessage());
    //  return result.ToPaginatedList();
    //}

    public async Task<ClinicDetailDto> GetClinic(int clinicId)
    {
      var result = await client.GetClinicAsync(new GetClinicReqeustMessage { ClinicId = clinicId });
      return result.Clinic.ToClinicDetailDto();
    }

    public async Task<ClinicItemDto> InsertClinic(ClinicInsertDto clinicToInsert)
    {
      var result = await client.InsertClinicAsync(clinicToInsert.ToInsertClinicReqeustMessage());
      return result.ClinicCreated.ToClinicItemDto();
    }

    public async Task<bool> UpdateClinic(ClinicUpdateDto clinicToUpdate)
    {
      var result = await client.UpdateClinicAsync(clinicToUpdate.ToUpdateClinicReqeustMessage());
      return result.Result;
    }

    public async Task<Paginatedlist<ClinicItemDto>> GetAllClinics(PaginationRequestDto dto)
    {
      var result = await client.GetClinicsAsync(new CommonRequestMessage { Content = JsonConvert.SerializeObject(dto)});
      var clinics = JsonConvert.DeserializeObject<Paginatedlist<ClinicItemDto>>(result.Content);
      return clinics;
    }
  }
}
