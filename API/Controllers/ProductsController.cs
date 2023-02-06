using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    [HttpGet]
    public string GetProducts()
    {
      return "TODO";
    }

    [HttpGet("{id}")]
    public string GetProduct(int id)
    {
      return "TODO";
    }
  }
}