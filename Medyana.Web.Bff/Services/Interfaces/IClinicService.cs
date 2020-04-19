using Dtos.Common;
using Medyana.Dtos.Clinic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Medyana.Web.Bff.Service
{
  public interface IClinicService
  {
    Task<Paginatedlist<ClinicItemDto>> GetAllClinics(PaginationRequestDto dto);
    Task<ClinicDetailDto> GetClinic(int clinicId);
    Task<ClinicItemDto> InsertClinic(ClinicInsertDto clinicToInsert);
    Task<bool> UpdateClinic(ClinicUpdateDto clinicToUpdate);
    Task<bool> DeleteClinic( int clinicId);
  }
}
