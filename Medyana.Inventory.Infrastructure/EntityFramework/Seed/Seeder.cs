using Medyana.Inventory.Domain.Entities;
using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Medyana.Inventory.Infrastructure.EntityFramework.Context
{
  public class Seeder
  {
    public Seeder(DataContext context)
    {
      this.context = context;
    }

    public DataContext context { get; }

    public bool Seed()
    {
      try
      {
        context.Database.EnsureCreated();
        if (!context.Clinics.Any())
        {
          if (context.Database.ProviderName.Contains(".InMemory") == false) // InMemory database is used for test purpose. InMemory DB is not a relational database.
            AddViews(context);
          AddClinics(context);
          AddEquipments(context);
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    private void AddViews(DataContext context)
    {
      context.Database.ExecuteSqlRaw(
                        @"CREATE VIEW ClinicSummary AS 
                            SELECT c.Id,c.Name, 
                                   (select Count(Id) FROM Equipments AS E WHERE E.ClinicId = C.Id) AS EquipmentCount
                            FROM Clinics AS C ");

    }

    private void AddClinics(DataContext context)
    {
      var clinic = new Clinic { Name = "MedIstanbul" };
      context.Clinics.Add(clinic);
      context.SaveChanges();

    }

    private void AddEquipments(DataContext context)
    {
      var firstClinic = context.Clinics.FirstOrDefault();
      if (firstClinic != null)
      {
        var equipments = new List<Equipment> {
          new Equipment { Name = "Sterilizier", ClinicId = firstClinic.Id, Quantity = 2, Price = 1000, UsageRate = 82, SupplyDate = DateTime.UtcNow.AddDays(30) },
          new Equipment { Name = "Stethoscope", ClinicId = firstClinic.Id, Quantity = 2, Price = 1000, UsageRate = 82, SupplyDate = DateTime.UtcNow.AddDays(20) }
        };
        context.Equipments.AddRange(equipments);
        context.SaveChanges();
      }

    }
  }
}
