using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Administration;
using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.OpenApi.Models;

namespace gateway
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc(option => option.EnableEndpointRouting = false);
      services.AddControllers();

      // services.AddSwaggerGen(c =>
      // {
      //   c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
      // });

      Action<IdentityServerAuthenticationOptions> options = o =>
      {
        o.Authority = Configuration["IS4:Authority"];
        o.ApiName = Configuration["IS4:ApiName"];
        o.ApiSecret = Configuration["IS4:ApiSecret"];
        o.SupportedTokens = SupportedTokens.Both;
        o.RequireHttpsMetadata = false; // ignorar https
      };

      services
        .AddAuthentication()
        .AddIdentityServerAuthentication(Configuration["IS4:ProviderKey"], options);

      services
        .AddOcelot()
        .AddAdministration("/administration", options);
    }

    async public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();
      app.UseAuthentication();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      // app.UseSwaggerUI(c =>
      // {
      //   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ocelot");
      // });

      await app.UseOcelot();
    }
  }
}
