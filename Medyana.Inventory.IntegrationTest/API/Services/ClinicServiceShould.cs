using Medyana.Inventory.API.Services;
using Medyana.Inventory.Domain.Interface;
using Medyana.Inventory.Infrastructure.Repositories;
using Medyana.Inventory.IntegrationTest.API.Common;
using System.Threading.Tasks;
using Xunit;
using Grpc.Core;
using Grpc.Core.Utils;
using System;
using System.Threading;
using System.Linq;
using Medyana.Inventory.IntegrationTest.API.DataSources;
using Medyana.Inventory.Domain.Entities;

namespace Medyana.Inventory.IntegrationTest.API.Services
{
  [Collection(nameof(DatabaseFixtureCollection))]
  public class ClinicServiceShould : DataOperationBaseShould
  {
    IClinicRepository clinicRepository { get; set; }
    ClinicAppService sut { get; set; }
    ServerCallContext callContext { get; set; }

    public ClinicServiceShould(DatabaseFixture dbFixture) : base(dbFixture)
    {
      clinicRepository = new ClinicRepository(Context);
      sut = new ClinicAppService(clinicRepository);
    }

    [Fact]
    public void GenericTestClassArrangements_IsSuccess()
    {
      Assert.NotNull(clinicRepository);
      Assert.NotNull(sut);
    }

    [Fact]
    public async Task GetClinic_Returns_When_ExistIdProvided()
    {
      var model = new GetClinicReqeustMessage { ClinicId = 1 };
      var result = await sut.GetClinic(model, default(ServerCallContext));
      Assert.Equal(model.ClinicId, result.Clinic.Id);
    }

    [Fact]
    public async Task GetClinic_ReturnsNull_When_AbsentIdProvided()
    {
      var model = new GetClinicReqeustMessage { ClinicId = 2000 };
      var result = await sut.GetClinic(model, default(ServerCallContext));
      Assert.Null(result.Clinic);
    }

    [Fact]
    public async Task DeleteClinic_IsSuccess()
    {
      try
      {
        var clinicToDelete = Context.Clinics.Add(new Clinic { Name = "Clinic to be deleted" });
        Context.SaveChanges();

        var clinicId = clinicToDelete.Entity.Id;

        var clinic = await sut.DeleteClinic( new DeleteClinicReqeustMessage { ClinicId = clinicId } , default(ServerCallContext));

        Assert.True(clinic.Result);
      }
      catch
      {
        Assert.True(false);
      }
    }

    #region DataDriven Tests

    [Theory]
    [MemberData(nameof(ClinicInternalTestData.ClinicsToInsert),
        MemberType = typeof(ClinicInternalTestData))]
    public async Task InsertClinic_DataDrivenTest(InsertClinicReqeustMessage model, bool expectation)
    {
      var clinic = await sut.InsertClinic(model, default(ServerCallContext));

      if (expectation)
        Assert.NotNull(clinic);
      else
        Assert.Null(clinic);
    }

    [Theory]
    [MemberData(nameof(ClinicInternalTestData.ClinicsToUpdate),
       MemberType = typeof(ClinicInternalTestData))]
    public async Task UpdateClinic_DataDrivenTest(UpdateClinicReqeustMessage dto, bool expectation)
    {
      try
      {
        var clinicId = Context.Clinics.First()?.Id;

        var clinic = await sut.UpdateClinic(new UpdateClinicReqeustMessage { ClinicId = clinicId.Value, Name = dto.Name }, default(ServerCallContext));

        if (expectation)
        {
          Assert.True(clinic.Result);
        }
        else
          Assert.False(clinic.Result);
      }
      catch
      {
        Assert.True(false);
      }
    }

    #endregion



  }
}
