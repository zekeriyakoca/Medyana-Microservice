﻿using Dtos.Common;
using Medyana.Dtos.Clinic;
using Medyana.IntegrationTest.Common;
using Medyana.IntegrationTest.DataSources;
using Medyana.Inventory.API.Services;
using Medyana.Inventory.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Medyana.IntegrationTest.Controllers
{
  [Collection(nameof(DatabaseFixtureCollection))]
  public class ClinicControllerShould : DataOperationBaseShould
  {
    public ClinicController clinicController { get; set; }

    public ClinicControllerShould(DatabaseFixture dbFixture) : base(dbFixture)
    {
      var logger = new Mock<ILogger<ClinicController>>();
      var clinicReposiyory = new ClinicRepository(Context);
      var clinicAppService = new ClinicAppService(clinicReposiyory);

      clinicController = new ClinicController(clinicAppService, logger.Object);
    }

    [Fact]
    public async Task GetClinic_IsSuccess()
    {
      try
      {
        var clinicId = Context.Clinics.First()?.Id;

        var clinic = await clinicController.GetClinic(clinicId.Value);
        Assert.NotNull(clinic);
      }
      catch
      {
        Assert.True(false);
      }
    }

    [Theory]
    [MemberData(nameof(ClinicInternalTestData.GetPaginatedClinicsReqeusts),
      MemberType = typeof(ClinicInternalTestData))]
    public async Task GetAllClinics_IsSuccess(PaginationRequestDto dto, bool expectation)
    {
      try
      {
        var okResult = await clinicController.GetClinics(dto) as OkObjectResult;

        //Asserts
        var result = Assert.IsType<OkObjectResult>(okResult);
        var clinics = result.Value;

        var model = Assert.IsAssignableFrom<Paginatedlist<ClinicItemDto>>(
            clinics);

        Assert.Equal(model.Items.Any(), expectation);
      }
      catch
      {
        Assert.True(false);
      }
    }
    [Theory]
    [MemberData(nameof(ClinicInternalTestData.ClinicsToInsert),
      MemberType = typeof(ClinicInternalTestData))]
    public async Task InsertClinic_IsSuccess(ClinicInsertDto dto, bool expectation)
    {
      try
      {
        var createdResult = await clinicController.AddClinic(dto) as CreatedResult;

        //Asserts
        var result = Assert.IsType<CreatedResult>(createdResult);
        var clinic = result.Value;

        var model = Assert.IsAssignableFrom<ClinicDetailDto>(
            clinic);

        if (expectation)
          Assert.NotNull(model);
        else
          Assert.Null(model);

      }
      catch
      {
        Assert.True(false);
      }
    }


    //TODO : implement ...
  }
}
