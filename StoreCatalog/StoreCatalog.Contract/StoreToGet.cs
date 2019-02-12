using System;

namespace GeekBurguer.StoreCatalog.Contract
{
    public class StoreToGet
    {
        public Guid StoreId { get; set; }
        public bool Ready { get; set; }
    }
}
