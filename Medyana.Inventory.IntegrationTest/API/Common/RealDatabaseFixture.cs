using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medyana.Inventory.IntegrationTest.API.Common
{
  public class RealDatabaseFixture
  {
    public IHostingEnvironment HostingEnvironment { get; set; }
    public DataContext Context { get; set; }

    public RealDatabaseFixture()
    {
      var mockEnvironment = new Mock<IHostingEnvironment>();
      mockEnvironment
               .Setup(m => m.ContentRootPath)
               .Returns("C:/Users/zeker/source/repos/Medyana - Microservice/Medyana.Test");
      this.HostingEnvironment = mockEnvironment.Object;

      var dbOptions = new DbContextOptionsBuilder<DataContext>()
                            .UseSqlServer("Data Source=DESKTOP-VUCGAKI\\SQLEXPRESS;Initial Catalog=Medyana;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                            .Options;
      
      Context = new DataContext(dbOptions);


      SeedDatabase();
    }

    protected void SeedDatabase()
    {
      var seeder = new Seeder(Context);
      seeder.Seed();
    }
  }
}
