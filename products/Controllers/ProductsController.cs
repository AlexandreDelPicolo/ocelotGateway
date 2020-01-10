using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace products.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private static readonly string[] products = new[]
    {
        "IphoneX", "Playstation", "Xbox"
    };

    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
      return products;
    }
  }
}
