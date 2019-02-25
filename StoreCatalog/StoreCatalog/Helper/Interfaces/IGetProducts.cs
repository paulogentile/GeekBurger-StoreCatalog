using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Helper
{
    public interface IGetProducts
    {
        Task<bool> RequestProducts();
    }
}