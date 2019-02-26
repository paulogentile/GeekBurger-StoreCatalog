using GeekBurger.StoreCatalog.Service;
using GeekBurger.StoreCatalog.Service.GetProductChanged;
using GeekBurger.StoreCatalog.Service.GetProductionAreaChanged;
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
        IGetProductChangedService _getProductChangedService;
        IGetProductionAreaChangedService _getProductionAreaChangedService;

        public AppInnit(IHealthCheck hc, IGetProducts gp, IGetProductions gpr, IStoreCatalogReadyService scrs, IGetProductChangedService gpcs, IGetProductionAreaChangedService gpacs)
        {
            _healthCheck = hc;
            _getProducts = gp;
            _getProductions = gpr;
            _catalogReadyMessage = scrs;
            _getProductChangedService = gpcs;
            _getProductionAreaChangedService = gpacs;
        }
        public void run()
        {
            //Pegar os Produtos
            _getProducts.RequestProducts().Wait();

            //Pegar as Areas de Produção
            _getProductions.RequestProductions().Wait();

            //Subscribe to ProductionAreaChanged
            _getProductionAreaChangedService.GetProductionAreaChanged();

            //Subscribe to ProductChanged
            _getProductChangedService.GetProductChanged();

            //Set System Healthy
            _healthCheck.Healthy = true;

            //Publish StoreCatalogReady
            _catalogReadyMessage.SendCatalogReady(_healthCheck.Healthy);
        }
    }
}
