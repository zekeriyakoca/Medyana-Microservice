using Dtos.Common;
using Medyana.Dtos.Clinic;
using Medyana.Inventory.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medyana.Web.Bff.Mappers
{
  public static class ClinicDtoStackMapperExtensions
  {

    public static Paginatedlist<ClinicItemDto> ToPaginatedList(this GetClinicsResponseMessage message)
    {
      return new Paginatedlist<ClinicItemDto>
      {
        Page = message.Page,
        PageSize = message.PageSize,
        TotalItemCount = message.TotalItemCount,
        Items = message.Items.Select(i => i.ToClinicItemDto()).ToList()
      };
    }

    public static ClinicItemDto ToClinicItemDto(this ClinicItemMessage message)
    {
      return new ClinicItemDto
      {
        Id = message.Id,
        Name = message.Name
      };
    }

    public static ClinicDetailDto ToClinicDetailDto(this ClinicDetailMessage message)
    {
      return new ClinicDetailDto
      {
        Id = message.Id,
        Name = message.Name
      };
    }
    public static InsertClinicReqeustMessage ToInsertClinicReqeustMessage(this ClinicInsertDto dto)
    {
      return new InsertClinicReqeustMessage
      {
        Name = dto.Name
      };
    }
    public static UpdateClinicReqeustMessage ToUpdateClinicReqeustMessage(this ClinicUpdateDto dto)
    {
      return new UpdateClinicReqeustMessage
      {
        ClinicId = dto.Id,
        Name = dto.Name
      };
    }
  }
}
