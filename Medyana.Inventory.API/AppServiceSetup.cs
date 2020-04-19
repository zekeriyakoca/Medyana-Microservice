using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Medyana.Inventory.API
{
  public static class AppServiceSetup
  {
    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContextPool<DataContext>(options =>
                options.UseSqlServer(configuration["DefaultConnection"]));

      IocInfrastructure.Set(services, configuration);

    }
  }
}
