using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System;

namespace GeekBurger.StoreCatalog.Service
{
    public interface IStoreCatalogReadyService
    {
        void SendCatalogReady(bool ready);       
    }
}