using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos.Common;
using Medyana.Dtos.Clinic;
using Medyana.Web.Bff.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Medyana.Web.Bff
{
  [Route("api/[controller]")]
  [ApiController]
  public class ClinicController : BaseController
  {
    public readonly ILogger<ClinicController> logger;
    public ClinicController(IClinicService clinicService, ILogger<ClinicController> logger) : base(logger)
    {
      this.clinicService = clinicService;
      this.logger = logger;
    }

    public IClinicService clinicService { get; }

    [HttpPost("")]
    public async Task<IActionResult> GetClinics([FromBody] PaginationRequestDto dto)
    {
      return await ActionHandle(async () =>
      {
        var clinics = await clinicService.GetAllClinics(dto);
        return Ok(clinics);
      });
      
    }

    [HttpGet("{clinicId}")]
    public async Task<IActionResult> GetClinic(int clinicId)
    {
      return await ActionHandle(async () =>
      {
        var clinic = await clinicService.GetClinic(clinicId);
        return Ok(clinic);
      });

    }

    [HttpPut("")]
    public async Task<IActionResult> AddClinic([FromBody]ClinicInsertDto clinic)
    {
      return await ActionHandle(async () =>
      {
        var createdClinic = await clinicService.InsertClinic(clinic);
        if (createdClinic == null)
          return BadRequest("Unable to insert clinic.");
        return Created($"api/clinic/{createdClinic.Id}",createdClinic);
      });
    }
    [HttpPatch("")]
    public async Task<IActionResult> UpdateClinic([FromBody]ClinicUpdateDto clinic)
    {
      return await ActionHandle(async () =>
      {
        var createdClinic = await clinicService.UpdateClinic(clinic);
        if (createdClinic == false)
          return BadRequest("Unable to update clinic.");
        return Ok(createdClinic);
      });
    }
    [HttpDelete("{clinicId}")]
    public async Task<IActionResult> DeleteClinic(int clinicId)
    {
      return await ActionHandle(async () =>
      {
        var createdClinic = await clinicService.DeleteClinic(clinicId);
        if (!createdClinic)
          return BadRequest("Unable to delete clinic.");
        return Ok(createdClinic);
      });
    }
  }
}