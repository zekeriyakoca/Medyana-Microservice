using Medyana.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medyana.Inventory.Domain.Interface
{
  public interface IEquipmentRepository : IRepository<Equipment>
  {
    Task<bool> Any(int id);

    Task<IEnumerable<Equipment>> GetAllIncludeAll();

  }
}
