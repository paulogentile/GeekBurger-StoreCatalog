using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.StoreCatalog.Controllers
{
    [Produces("application/json")]
    [Route("api/store")]
    public class StoreController : Controller
    {
        /// <summary>
        /// Método que checa se a produção está pronta
        /// </summary>
        /// <returns>Retorna se a loja está pronta para produção</returns>
        [HttpGet]
        public IActionResult GetStore()
        {
            var a = new StoreToGet();

            return Ok(a);
        }
    }
}