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
using Ocelot.Authorisation;
using System.Security.Claims;
using System.Linq;
using System.Text.RegularExpressions;

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

      Action<IdentityServerAuthenticationOptions> options = o =>
      {
        o.Authority = Configuration["IS4:Authority"];
        o.ApiName = Configuration["IS4:ApiName"];
        o.ApiSecret = Configuration["IS4:ApiSecret"];
        o.SupportedTokens = SupportedTokens.Both;
        o.RequireHttpsMetadata = false; // ignorar https
      };

      //services.AddAuthorization();
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

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      // Pipeline de auth customizado para múltiplas roles - Comentar para voltar para o default
      var configuration = new OcelotPipelineConfiguration
      {
        AuthorisationMiddleware = async (ctx, next) =>
        {
          if (CustomAuthenticationPipeline.Execute(ctx))
            await next.Invoke();
          else
            ctx.Errors.Add(new UnauthorisedError($"Fail to authorize"));
        }
      };

      await app.UseOcelot(configuration);
    }
  }
}
