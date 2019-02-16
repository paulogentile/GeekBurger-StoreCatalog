using GeekBurger.StoreCatalog.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Repository
{
    public class StoreCatalogContext : DbContext
    {
        public StoreCatalogContext(DbContextOptions<StoreCatalogContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Area> Areas { get; set; }
    }
}
