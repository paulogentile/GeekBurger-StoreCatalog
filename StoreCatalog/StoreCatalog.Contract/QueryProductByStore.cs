using System;

namespace GeekBurger.StoreCatalog.Contract
{
    public class QueryProductByStore
    {
        public string StoreName { get; set; }
        public Guid UserId { get; set; }
        public string[] Restrictions { get; set; }
    }
}
