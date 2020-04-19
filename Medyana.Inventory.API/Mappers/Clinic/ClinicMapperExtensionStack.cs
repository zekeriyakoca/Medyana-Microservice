using Medyana.Inventory.Domain.Entities;
using Medyana.Dtos.Clinic;
using Medyana.Inventory.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medyana.Inventory.API.Mappers
{
  public static class ClinicMapperExtensionStack
  {
    public static ClinicDetailMessage ToClinicDetailMessage(this Clinic model)
    {
      return new ClinicDetailMessage()
      {
        Id = model.Id,
        Name = model.Name
      };
    }

    public static Clinic ToClinic(this InsertClinicReqeustMessage message)
    {
      return new Clinic()
      {
        Name = message.Name
      };
    }

    public static Clinic ToClinic(this UpdateClinicReqeustMessage message, Clinic clinic)
    {
      return new Clinic()
      {
        Id = message.ClinicId,
        Name = message.Name
      };
    }

    public static ClinicItemMessage ToClinicItemMessage(this Clinic model)
    {
      return new ClinicItemMessage()
      {
        Id = model.Id,
        Name = model.Name
      };
    }
    public static List<ClinicItemMessage> ToClinicItemMessageList(this IEnumerable<Clinic> modelList)
    {
      return modelList.Select(m => m.ToClinicItemMessage()).ToList();
    }

    public static void Parse(this Clinic clinic, UpdateClinicReqeustMessage dto)
    {
      if (clinic.Id != dto.ClinicId)
        throw new Exception("ClinicUpdateDto parse - Model and dto should have the same id");
      clinic.Name = dto.Name;
    }

    public static Clinic ToClinic(this ClinicInsertDto dto)
    {
      return new Clinic()
      {
        Name = dto.Name
      };
    }

    public static Clinic ToClinic(this ClinicUpdateDto dto, Clinic clinic)
    {
      return new Clinic()
      {
        Id = dto.Id,
        Name = dto.Name
      };
    }

    public static ClinicItemDto ToClinicItemDto(this Clinic model)
    {
      return new ClinicItemDto()
      {
        Id = model.Id,
        Name = model.Name
      };
    }
    public static List<ClinicItemDto> ToClinicItemDtoList(this IEnumerable<Clinic> modelList)
    {
      return modelList.Select(m => m.ToClinicItemDto()).ToList();
    }

    public static ClinicDetailDto ToClinicDetailDto(this Clinic model)
    {
      return new ClinicDetailDto()
      {
        Id = model.Id,
        Name = model.Name,
        Equipments = model.Equipments?.ToEquipmentItemDtoList()
      };
    }

    public static void Parse(this Clinic clinic, ClinicUpdateDto dto)
    {
      if (clinic.Id != dto.Id)
        throw new Exception("ClinicUpdateDto parse - Model and dto should have the same id");
      clinic.Name = dto.Name;
    }
  }
}
