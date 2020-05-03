using Medyana.Inventory.Domain.Entities;
using Medyana.Inventory.Domain.Interface;
using Medyana.Inventory.Infrastructure.EntityFramework.Context;
using Medyana.Inventory.Infrastructure.Repositories;
using Medyana.Inventory.IntegrationTest.API.Common;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medyana.Inventory.IntegrationTest.API
{

  public class TestShould : IClassFixture<RealDatabaseFixture>
  {
    IClinicRepository clinicRepository { get; set; }
    IHostingEnvironment HostingEnvironment { get; set; }
    DataContext Context { get; set; }

    private readonly ITestOutputHelper console;

    public TestShould(RealDatabaseFixture dbFixture, ITestOutputHelper testOutputHelper)
    {
      this.HostingEnvironment = dbFixture.HostingEnvironment;
      this.Context = dbFixture.Context;
      clinicRepository = new ClinicRepository(Context);
      this.console = testOutputHelper;

    }

  }
}
