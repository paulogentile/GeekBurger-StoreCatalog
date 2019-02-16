using System;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.StoreCatalog.Model
{
    public class Area
    {
        [Key]
        public Guid AreaId { get; set; }

        public Guid ProductionId { get; set; }

        public string[] Restrictions { get; set; }

        public bool On { get; set; }
    }
}
