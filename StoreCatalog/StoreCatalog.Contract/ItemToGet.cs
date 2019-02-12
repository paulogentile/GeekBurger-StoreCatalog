using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.StoreCatalog.Contract
{
    public class ItemToGet
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
    }
}
