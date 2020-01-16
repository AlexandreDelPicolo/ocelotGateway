using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Ocelot.Middleware;

namespace gateway
{
  public static class CustomAuthenticationPipeline
  {
    public static bool Execute(DownstreamContext ctx)
    {
      bool hasRole = ctx.DownstreamReRoute.RouteClaimsRequirement.TryGetValue("role", out string routeClaimsRequirementValue);

      if (!hasRole)
        return true;

      Claim[] claims = ctx.HttpContext.User.Claims
                                           ?.Where(x => x.Type.Equals("role", StringComparison.InvariantCultureIgnoreCase))
                                           ?.ToArray<Claim>();

      Regex regexOr = new Regex(@"[^,\s+$ ][^\,]*[^,\s+$ ]");
      MatchCollection matches = regexOr.Matches(routeClaimsRequirementValue);

      foreach (Match match in matches)
      {
        if ((claims?.Any(x => x.Value == match.Value)) ?? false)
          return true;
      }

      return false;
    }
  }
}
