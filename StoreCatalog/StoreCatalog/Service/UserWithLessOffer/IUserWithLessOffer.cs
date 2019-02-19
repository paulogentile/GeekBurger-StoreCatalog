using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Service.UserWithLessOffer
{
    public interface IUserWithLessOffer
    {
        void SendUserWithLessOffer(Guid userId, string[] restrictions);
    }
}
