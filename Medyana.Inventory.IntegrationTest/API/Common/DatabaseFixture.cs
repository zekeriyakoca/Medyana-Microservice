using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medyana.Inventory.IntegrationTest.API.Common
{
  public class DatabaseFixture
  {
    public IHostingEnvironment HostingEnvironment { get; set; }
    public DataContext Context { get; set; }

    public DatabaseFixture()
    {
      var mockEnvironment = new Mock<IHostingEnvironment>();
      mockEnvironment
               .Setup(m => m.ContentRootPath)
               .Returns("C:/Users/zeker/source/repos/Medyana - Microservice/Medyana.Test");
      this.HostingEnvironment = mockEnvironment.Object;

      var dbOptions = new DbContextOptionsBuilder<DataContext>()
                            .UseInMemoryDatabase(databaseName: "MedyanaTestDBContext")
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
