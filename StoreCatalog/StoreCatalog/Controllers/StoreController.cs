using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeekBurger.StoreCatalog.Controllers
{
    [Produces("application/json")]
    [Route("api/store")]
    public class StoreController : Controller
    {
        private IConfiguration _configuration;
        private IHealthCheck _healthCheck;

        public StoreController(IConfiguration configuration, IHealthCheck healthCheck)
        {
            _configuration = configuration;
            _healthCheck = healthCheck;
        }

        /// <summary>
        /// Método que checa se a produção está pronta
        /// </summary>
        /// <returns>Retorna se a loja está pronta para produção</returns>
        /// 
        [HttpGet]
        public IActionResult GetStore()
        {
            var storeID = _configuration.GetSection("Store:Id").Get<Guid>();

            var a = new StoreToGet
            {
                StoreId = storeID,
                Ready = _healthCheck.Healthy
            };

            return Ok(a);
        }
    }
}