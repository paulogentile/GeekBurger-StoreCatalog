using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurguer.StoreCatalog.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


//teste
namespace GeekBurguer.StoreCatalog.Controllers
{
    [Produces("application/json")]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public IActionResult GetProductsByStore(QueryProductByStore dados)
        {
            var a = new ProductByStoreToGet();

            return Ok(a);
        }
    }
}