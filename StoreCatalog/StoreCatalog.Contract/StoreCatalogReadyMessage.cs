using System;

namespace GeekBurguer.StoreCatalog.Contract
{
    public class StoreCatalogReadyMessage
    {
        public Guid StoreId { get; set; }
        public bool Ready { get; set; }
    }
}
