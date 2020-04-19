﻿using Medyana.Inventory.Domain.Entities;
using Medyana.Inventory.Domain.Interface;
using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medyana.Inventory.Infrastructure.Repositories
{
  public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
  {
    public EquipmentRepository(DataContext context) : base(context)
    {

    }

    public async Task<bool> Any(int id)
    {
      return await Context.Set<Equipment>().AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Equipment>> GetAll()
    {
      return Context.Set<Equipment>().AsNoTracking();
    }

    public async Task<IEnumerable<Equipment>> GetAllIncludeAll()
    {
      return Context.Set<Equipment>().Include(e=>e.Clinic).AsNoTracking();
    }
  }
}
