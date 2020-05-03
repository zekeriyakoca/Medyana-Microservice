using Grpc.Core;
using Medyana.Dtos.Clinic;
using Medyana.Dtos.Equipment;
using Medyana.Inventory.API.Services;
using Medyana.Inventory.Domain.Entities;
using Medyana.Inventory.Infrastructure.Repositories;
using Medyana.Inventory.IntegrationTest.API.Common;
using Medyana.Inventory.IntegrationTest.API.DataSources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Medyana.Inventory.IntegrationTest.API.Services
{
  [Collection(nameof(DatabaseFixtureCollection))]
  public class EquipmentServiceShould : DataOperationBaseShould
  {
    public EquipmentAppService sut;
    public EquipmentRepository equipmentRepository;

    public EquipmentServiceShould(DatabaseFixture fixture) : base(fixture)
    {
      var clinicRepository = new ClinicRepository(Context);
      equipmentRepository = new EquipmentRepository(Context);
      sut = new EquipmentAppService(equipmentRepository, clinicRepository);
    }

    [Fact]
    public void GenericTestClassArrangements_IsSuccess()
    {
      Assert.NotNull(sut);
    }

    [Fact]
    public async Task GetEquipment_Returns_When_IdExist()
    {
      var equipmentId = 1;

      var requestModel = new CommonRequestMessage { Content = JsonConvert.SerializeObject(equipmentId) };

      var result = await sut.GetEquipment(requestModel, default(ServerCallContext));

      var equipment = JsonConvert.DeserializeObject<EquipmentDetailDto>(result.Content);

      Assert.True(result.Status);
      Assert.NotNull(equipment);
      Assert.Equal(equipment.Id, equipmentId);

    }

    [Fact]
    public async Task GetEquipment_ReturnsNull_When_IdAbsent()
    {
      var equipmentId = int.MaxValue;

      var requestModel = new CommonRequestMessage { Content = JsonConvert.SerializeObject(equipmentId) };

      var result = await sut.GetEquipment(requestModel, default(ServerCallContext));

      var equipment = JsonConvert.DeserializeObject<EquipmentDetailDto>(result.Content);

      Assert.True(result.Status);
      Assert.Null(equipment);

    }

    [Fact]
    public async Task DeleteEquipment_IsSuccess()
    {

      var clinicToClone = equipmentRepository.GetAll().Result.First();
      var clinicToInsert = JsonConvert.DeserializeObject<Equipment>(JsonConvert.SerializeObject(clinicToClone));
      clinicToInsert.Name = "Equipment to be deleted";
      clinicToInsert.Id = 0;
      equipmentRepository.Add(clinicToInsert);
      equipmentRepository.SaveChanges();

      var requestModel = new CommonRequestMessage { Content = JsonConvert.SerializeObject(clinicToInsert.Id) };

      var result = await sut.DeleteEquipment(requestModel, default(ServerCallContext));

      Assert.True(result.Status);
    }

    #region DataDriven Tests

    [Theory]
    [MemberData(nameof(EquipmentInternalTestData.EquipmentsToInsert),
        MemberType = typeof(EquipmentInternalTestData))]
    public async Task InsertEquipment_DataDrivenTest(EquipmentInsertDto model, bool expectation)
    {
      var requestModel = new CommonRequestMessage { Content = JsonConvert.SerializeObject(model) };

      CommonResponseMessage response = await sut.InsertEquipment(requestModel, default(ServerCallContext));

      var equipment = JsonConvert.DeserializeObject<EquipmentDetailDto>(response.Content);

      if (expectation)
      {
        Assert.True(response.Status);
        Assert.NotNull(equipment);
        Assert.Equal(equipment.Name, model.Name);
      }
      else
      {
        Assert.Null(equipment);
      }
    }

    [Theory]
    [MemberData(nameof(EquipmentInternalTestData.EquipmentsToUpdate),
       MemberType = typeof(EquipmentInternalTestData))]
    public async Task UpdateEquipment_DataDrivenTest(EquipmentUpdateDto model, bool expectation)
    {
      try
      {
        var equipmentId = Context.Equipments.First()?.Id;

        model.Id = equipmentId.Value;
        var requestModel = new CommonRequestMessage { Content = JsonConvert.SerializeObject(model) };

        CommonResponseMessage response = await sut.UpdateEquipment(requestModel, default(ServerCallContext));

        var equipment = JsonConvert.DeserializeObject<EquipmentDetailDto>(response.Content);

        if (expectation)
        {
          Assert.True(response.Status);
          Assert.Equal(equipment.Id, model.Id);
        }
        else { 
          Assert.False(response.Status);
        }
      }
      catch
      {
        Assert.True(false);
      }
    }

    #endregion


  }
}
