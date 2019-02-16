using GeekBurger.StoreCatalog.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Repository
{
    public static class StoreCatalogContextExtensions
    {
        public static void Seed(this StoreCatalogContext context)
        {
            var productsTxt = File.ReadAllText("products.json");
            var products = JsonConvert.DeserializeObject<List<Product>>(productsTxt);
            context.Products.AddRange(products);

            context.SaveChanges();
        }
    }
}
