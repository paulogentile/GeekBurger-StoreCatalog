using GeekBurger.StoreCatalog.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using System;

namespace GeekBurger.StoreCatalog.Controllers
{
    public class StoreController : Controller
    {
        private IConfiguration _configuration;
        private IHealthCheck _healthCheck;
        private readonly ILogger _logger;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public StoreController(
            IConfiguration configuration, 
            IHealthCheck healthCheck)
        {
            _configuration = configuration;
            _healthCheck = healthCheck;
        }

        /// <summary>
        /// Método que checa se a produção está pronta
        /// </summary>
        /// <returns>Retorna se a loja está pronta para produção</returns>
        [Produces("application/json")]
        [Route("api/store")]
        [HttpGet]
        public IActionResult GetStore()
        {
            var storeID = _configuration.GetSection("Store:Id").Get<Guid>();

            if (_healthCheck.Healthy)
            {
                return Ok(new StoreToGet
                {
                    StoreId = storeID,
                    Ready = _healthCheck.Healthy
                });
            }
            else
                return StatusCode(503);
        }
    }
}