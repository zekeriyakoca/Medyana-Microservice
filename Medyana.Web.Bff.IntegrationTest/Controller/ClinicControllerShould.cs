using Dtos.Common;
using Medyana.Dtos.Clinic;
using Medyana.Web.Bff.IntegrationTest.Common;
using Medyana.Web.Bff.IntegrationTest.DataSources;
using Medyana.Web.Bff.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Medyana.Web.Bff.IntegrationTest.Controller
{
  public class ClinicControllerShould : BaseControllerShould
  {
    private readonly ClinicController sut;

    public ClinicControllerShould()
    {
      var settings = new AppSettings()
      {
        InventoryApiUrl = "https://localhost:5001"
      };
      IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
      var options = new Mock<IOptionsSnapshot<AppSettings>>();
      options.Setup(o => o.Value).Returns(settings);
      var logger = new Mock<ILogger<ClinicController>>();
      var clinicService = new ClinicService(options.Object);
      sut = new ClinicController(clinicService, logger.Object);
    }

    [Fact]
    public void GenericTestClassArrangements_IsSuccess()
    {
      Assert.NotNull(sut);
    }

    [Fact]
    public async Task GetClinic_IsSuccess()
    {
      try
      {
        var clinicId = 1;

        var clinic = await sut.GetClinic(clinicId);
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
        var okResult = await sut.GetClinics(dto) as OkObjectResult;

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
        var createdResult = await sut.AddClinic(dto) as CreatedResult;

        //Asserts
        var result = Assert.IsType<CreatedResult>(createdResult);
        var clinic = result.Value;

        var model = Assert.IsAssignableFrom<ClinicItemDto>(
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


  }
}
