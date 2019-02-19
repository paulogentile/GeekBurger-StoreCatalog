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
        public AppInnit(IHealthCheck hc, IGetProducts gp)
        {
            _healthCheck = hc;
            _getProducts = gp;
        }
        public void run()
        {
            //TODO: Pegar as Areas de Produção

            //TODO: Pegar os Produtos
            _getProducts.RequestProducts().Wait();

            //TODO: Subscribe to ProductionAreaChanged

            //TODO: Subscribe to ProductChanged

            //TODO: Publish StoreCatalogReady

            _healthCheck.Healthy = true;
        }
    }
}
