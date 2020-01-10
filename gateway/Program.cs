using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace gateway
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
              config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile($"settings/appsettings.json", false, true)
                .AddJsonFile($"settings/appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddJsonFile($"routes/ocelot.json", false, true)
                .AddJsonFile($"routes/ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
