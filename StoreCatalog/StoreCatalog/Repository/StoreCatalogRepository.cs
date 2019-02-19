using GeekBurger.StoreCatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using GeekBurger.Production.Contract;
using GeekBurger.Products.Contract;
using AutoMapper;

namespace GeekBurger.StoreCatalog.Repository
{
    public class StoreCatalogRepository : IStoreCatalogRepository
    {
        private StoreCatalogContext _context;
        private IMapper _mapper;

        public StoreCatalogRepository(StoreCatalogContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }

        public IEnumerable<Product> GetProductsByRestrictions(List<Item> restrictions)
        {
            throw new NotImplementedException();
        }

        public void UpsertProduct(ProductToGet product)
        {
            var produto = _mapper.Map<Product>(product);

            _context.Products.Add(produto);
            _context.SaveChanges();
        }

        public void UpsertProduction(ProductionToGet production)
        {
            throw new NotImplementedException();
        }
    }
}
