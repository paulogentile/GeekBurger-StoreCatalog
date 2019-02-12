using System;

namespace GeekBurguer.StoreCatalog.Contract
{
    public class UserWithLessOfferMessage
    {
        public Guid UserId { get; set; }
        public string[] Restrictions { get; set; }
    }
}
