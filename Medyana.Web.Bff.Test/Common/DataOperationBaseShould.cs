using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Medyana.Inventory.IntegrationTest.API.Common
{
  [CollectionDefinition(nameof(DatabaseFixtureCollection))]
  public class DataOperationBaseShould // :IClassFixture<DatabaseFixture>
  {
    public IHostingEnvironment HostingEnvironment { get; set; }

    public DataContext Context { get; set; }

    public DataOperationBaseShould(DatabaseFixture dbFixture)
    {
      this.HostingEnvironment = dbFixture.HostingEnvironment;
      this.Context = dbFixture.Context;
    }
  }
}
