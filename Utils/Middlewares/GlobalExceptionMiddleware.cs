using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Middlewares
{
  public class GlobalExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger logger;
    public GlobalExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
      _next = next;
      logger = loggerFactory.CreateLogger<GlobalExceptionMiddleware>();
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Unhandled exception");
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      return context.Response.WriteAsync(new ErrorDetails()
      {
        StatusCode = context.Response.StatusCode,
        Message = "Internal Server Error."
      }.ToString());
    }
  }
  public class ErrorDetails {
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
      return JsonConvert.SerializeObject(this);
    }
  }
}
