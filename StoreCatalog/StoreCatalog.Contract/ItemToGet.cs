using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurguer.StoreCatalog.Contract
{
    public class ItemToGet
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
    }
}
