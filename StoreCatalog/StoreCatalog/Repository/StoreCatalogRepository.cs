using GeekBurger.StoreCatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using GeekBurger.Production.Contract;
using GeekBurger.Products.Contract;

namespace GeekBurger.StoreCatalog.Repository
{
    public class StoreCatalogRepository : IStoreCatalogRepository
    {
        private StoreCatalogContext _context;
        public StoreCatalogRepository(StoreCatalogContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProductsByRestrictions(List<Item> restrictions)
        {
            throw new NotImplementedException();
        }

        public void UpsertProduct(ProductToGet product)
        {
            throw new NotImplementedException();
        }

        public void UpsertProduction(ProductionToGet production)
        {
            throw new NotImplementedException();
        }
    }
}
