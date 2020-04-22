using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Medyana.Inventory.Domain.Entities
{
  public class ClinicSummary
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int EquipmentCount { get; set; }
  }

}
