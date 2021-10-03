using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ElectronAutoUpdateApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
      .ConfigureAppConfiguration((hostContext, builder) =>
      {
        // Add other providers for JSON, etc.
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
          builder.AddUserSecrets<Program>();
        }
      })
      .ConfigureWebHostDefaults(webBuilder =>
      {
        webBuilder.UseStartup<Startup>();
      });
  }
}
