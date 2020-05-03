using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medyana.Web.Bff.Services
{
  public class BaseGrpcService<T> where T : ClientBase<T>
  {
    private static object _syncLock = new object();
    private T _client { get; set; }

    public T client
    {
      get
      {
        if (_client == null)
        {
          lock (_syncLock)
          {
            if (_client == null)
            {
              var channel = GrpcChannel.ForAddress(appSettings.InventoryApiUrl);
              _client = (T)Activator.CreateInstance(typeof(T), channel);
            }
          }
        }
        return _client;
      }
    }

    private readonly AppSettings appSettings;
    public BaseGrpcService(IOptionsSnapshot<AppSettings> settings)
    {
      this.appSettings = settings.Value;
    }
  }
}
