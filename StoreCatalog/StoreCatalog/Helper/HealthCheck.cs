using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog
{
    public class HealthCheck : IHealthCheck
    {
        public bool Healthy { get; set; } = false;

        public HealthCheck()
        {
            //TODO: Pegar as Areas de Produção

            //TODO: Pegar os Produtos

            //TODO: Subscribe to ProductionAreaChanged

            //TODO: Subscribe to ProductChanged

            //TODO: Publish StoreCatalogReady

            Healthy = true;
        }
    }
}
