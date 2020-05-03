﻿using Castle.Core.Configuration;
using Dtos.Common;
using Medyana.Dtos.Clinic;
using Medyana.IntegrationTest.Common;
using Medyana.IntegrationTest.DataSources;
using Medyana.Inventory.API.Services;
using Medyana.Inventory.Infrastructure.Repositories;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Medyana.IntegrationTest.AppServices
{
  /// <summary>
  /// Tests Depends on Database Seed.
  /// Delete Operations should depends on instantly created data
  /// </summary>

  [Collection(nameof(DatabaseFixtureCollection))]
  public class ClinicAppServiceShould : DataOperationBaseShould
  {
    public ClinicAppService clinicAppService { get; set; }
    public ClinicRepository clinicRepository { get; set; }


    public ClinicAppServiceShould(DatabaseFixture dbFixture) : base(dbFixture)
    {
      var configuration = new Mock<IConfiguration>();
      var serviceProvider = new Mock<IServiceProvider>();

      clinicRepository = new ClinicRepository(base.Context);
      clinicAppService = new ClinicAppService(clinicRepository);
    }

    [Fact]
    public void GenericTestClassArrangements_IsSuccess()
    {
      Assert.NotNull(clinicAppService);
      Assert.NotNull(clinicRepository);
    }

    [Fact]
    public async Task GetClinic_IsSuccess()
    {
      try
      {
        var clinicId = Context.Clinics.First()?.Id;

        var clinic = await clinicAppService.GetClinic(clinicId.Value);
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
        var clinic = await clinicAppService.GetAllClinics(dto);

        Assert.Equal(clinic.Items.Any(), expectation);

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
        var clinic = await clinicAppService.InsertClinic(dto);

        if (expectation)
          Assert.NotNull(clinic);
        else
          Assert.Null(clinic);
      }
      catch
      {
        Assert.True(false);
      }
    }

    [Theory]
    [MemberData(nameof(ClinicInternalTestData.ClinicsToUpdate),
        MemberType = typeof(ClinicInternalTestData))]
    public async Task UpdateClinic_IsSuccess(ClinicInsertDto dto, bool expectation)
    {
      try
      {
        var clinicId = Context.Clinics.First()?.Id;

        var clinic = await clinicAppService.UpdateClinic(new Dtos.Clinic.ClinicUpdateDto { Name = dto.Name, Id = clinicId.Value });

        if (expectation)
        {
          Assert.NotNull(clinic);
          Assert.Equal(clinic.Id, clinicId);
        }
        else
          Assert.Null(clinic);
      }
      catch
      {
        Assert.True(false);
      }
    }

    [Fact]
    public async Task DeleteClinicIsSuccess()
    {
      try
      {
        var clinicToDelete = Context.Clinics.Add(new Clinic { Name = "Clinic to be deleted" });
        Context.SaveChanges();

        var clinicId = clinicToDelete.Entity.Id;

        var clinic = await clinicAppService.DeleteClinic(clinicId);

        Assert.True(clinic);
      }
      catch
      {
        Assert.True(false);
      }
    }


  }
}
