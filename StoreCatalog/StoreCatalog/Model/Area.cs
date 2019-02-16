using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.StoreCatalog.Model
{
    public class Area
    {
        [Key]
        public Guid AreaId { get; set; }

        public Guid ProductionId { get; set; }

        public ICollection<Restrictions> Restrictions { get; set; } = new List<Restrictions>();

        public bool On { get; set; }
    }
}
