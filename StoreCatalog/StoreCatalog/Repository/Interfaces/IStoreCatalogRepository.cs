using GeekBurger.StoreCatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Products.Contract;
using GeekBurger.Production.Contract;

namespace GeekBurger.StoreCatalog.Repository.Interfaces
{
    public interface IStoreCatalogRepository
    {
        IEnumerable<Product> GetProductsByRestrictions(string[] restrictions);
        void UpsertProduct(ProductToGet product);
        void UpsertProduction(ProductionToGet production);
        IQueryable<Area> GetAreas();
    }
}
