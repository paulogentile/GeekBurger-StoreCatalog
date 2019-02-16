using AutoMapper;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Product, ProductByStoreToGet>();
            CreateMap<Item, ItemToGet>();
        }
    }
}
