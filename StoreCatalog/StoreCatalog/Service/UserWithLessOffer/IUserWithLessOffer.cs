using System;

namespace GeekBurger.StoreCatalog.Service.UserWithLessOffer
{
    public interface IUserWithLessOffer
    {
        void SendUserWithLessOffer(Guid userId, string[] restrictions);
    }
}
