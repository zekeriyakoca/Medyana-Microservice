using Medyana.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medyana.Inventory.Infrastructure.EntityFramework.Context
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<ClinicSummary> ClinicSummaries { get; set; } // View

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ClinicSummary>(eb =>
        {
          eb.HasNoKey();
          eb.ToView("ClinicSummary");
        });
      base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (optionsBuilder.IsConfigured)
      {
        return;
      }
    }

  }

}
