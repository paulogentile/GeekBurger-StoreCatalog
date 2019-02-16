using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog
{
    public interface IHealthCheck
    {
        bool Healthy { get; set; }
    }
}
