using GeekBurger.Production.Contract;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekBurger.StoreCatalog.Repository
{
    public class StoreCatalogRepository : IStoreCatalogRepository
    {
        private StoreCatalogContext _context;
        private IConfiguration _configuration;

        public StoreCatalogRepository(StoreCatalogContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IEnumerable<Product> GetProductsByRestrictions(string[] restrictions)
        {
            return _context.Products.Include(p => p.Ingredients).Where(p => !p.Ingredients.Any(c => restrictions.Contains(c.Name)));
        }

        public void UpsertProduct(ProductToGet product)
        {
            var p = _context.Products.Find(product.ProductId);
            if (p == null)
            {
                p = new Product();
                _context.Products.Add(p);

                p.ProductId = product.ProductId;
                p.StoreId = _configuration.GetSection("Store:Id").Get<Guid>();
            }

            p.Image = product.Image;
            p.Name = product.Name;
            p.Price = product.Price;

            p.Ingredients.Clear();
            foreach (var item in product.Items)
                p.Ingredients.Add(new Item { ItemId = item.ItemId, Name = item.Name });

            _context.SaveChanges();
        }

        public void UpsertProduction(ProductionToGet production)
        {
            var p = _context.Areas.Find(production.ProductionId);
            if (p == null)
            {
                p = new Area();
                _context.Areas.Add(p);

                p.AreaId = production.ProductionId;
            }

            p.On = production.On == true;

            p.Restrictions.Clear();
            foreach (var item in production.Restrictions)
                p.Restrictions.Add(new Restrictions { Name = item });

            _context.SaveChanges();
        }

        public IQueryable<Area> GetAreas()
        {
            return _context.Areas.Include(p => p.Restrictions);
        }
    }
}
