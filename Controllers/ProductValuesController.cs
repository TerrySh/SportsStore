using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers {

  [Route("api/products")]
  public class ProductValuesController : Controller {
    private DataContext context;

    public ProductValuesController(DataContext ctx) {
      context = ctx;
    }


    [HttpGet("{id}")]
    public Product GetProduct(long id) {
      return context.Products.Find(id);
    }
  }

}
