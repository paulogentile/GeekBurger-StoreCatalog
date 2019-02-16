using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Model
{
    public class Restrictions
    {
        [Key]
        public Guid RestrictionId { get; set; }
        public string Name { get; set; }
    }
}
