using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace sales.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class SalesController : ControllerBase
  {
    private static readonly string[] sales = new[]
    {
        "Test1", "Test2", "Testt3"
    };

    private static readonly string[] sales2 = new[]
    {
        "Test4", "Test5", "Testt6"
    };

    private readonly ILogger<SalesController> _logger;

    public SalesController(ILogger<SalesController> logger)
    {
      _logger = logger;
    }

    [HttpGet("1")]
    public IEnumerable<string> Get()
    {
      return sales;
    }

    [HttpGet("2")]
    public IEnumerable<string> Get2()
    {
      return sales2;
    }
  }
}