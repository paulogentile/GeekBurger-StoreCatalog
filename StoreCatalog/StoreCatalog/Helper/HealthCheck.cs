using AutoMapper;
using GeekBurger.StoreCatalog.Helper;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog
{
    public class HealthCheck : IHealthCheck
    {
        public bool Healthy { get; set; } = false;

        public HealthCheck(IGetProducts gp)
        {
            //TODO: Pegar as Areas de Produção

            //TODO: Pegar os Produtos
            gp.RequestProducts();

            //TODO: Subscribe to ProductionAreaChanged

            //TODO: Subscribe to ProductChanged

            //TODO: Publish StoreCatalogReady

            Healthy = true;
        }
    }
}
