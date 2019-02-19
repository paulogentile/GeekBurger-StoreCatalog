using GeekBurger.StoreCatalog.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Helper
{
    public class AppInnit : IAppInnit
    {
        IHealthCheck _healthCheck;
        IGetProducts _getProducts;
        IGetProductions _getProductions;
        IStoreCatalogReadyService _catalogReadyMessage;

        public AppInnit(IHealthCheck hc, IGetProducts gp, IGetProductions gpr, IStoreCatalogReadyService scrs)
        {
            _healthCheck = hc;
            _getProducts = gp;
            _getProductions = gpr;
            _catalogReadyMessage = scrs;
        }
        public void run()
        {

            //Pegar os Produtos
            _getProducts.RequestProducts().Wait();

            //Pegar as Areas de Produção
            _getProductions.RequestProductions().Wait();

            //TODO: Subscribe to ProductionAreaChanged

            //TODO: Subscribe to ProductChanged

            _healthCheck.Healthy = true;

            //Publish StoreCatalogReady
            _catalogReadyMessage.SendCatalogReady(_healthCheck.Healthy);
        }
    }
}
