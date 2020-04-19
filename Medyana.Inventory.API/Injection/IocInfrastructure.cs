using Medyana.Inventory.Domain.Interface;
using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using Medyana.Inventory.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Medyana.Inventory.API
{
  public class IocInfrastructure
  {
    public static void Set(IServiceCollection services, IConfiguration configuration) {


      services.AddSingleton<IConfiguration>(configuration);
      services.AddSingleton<IServiceCollection>(services);

      services.AddTransient<Seeder>();

      services.AddScoped<IClinicRepository, ClinicRepository>();
      services.AddScoped<IEquipmentRepository, EquipmentRepository>();


    }
  }
}
