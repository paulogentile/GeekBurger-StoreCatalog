using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Model;
//using GeekBurger.StoreCatalog.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Service
{
    public interface IProductChangedService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<EntityEntry<Product>> changes);
    }
}