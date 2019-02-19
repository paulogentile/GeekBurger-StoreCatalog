using AutoMapper;
using GeekBurger.StoreCatalog.Helper;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog
{
    public class HealthCheck : IHealthCheck
    {
        public bool Healthy { get; set; } = false;
    }
}
