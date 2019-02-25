using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Helper
{
    public interface IGetProductions
    {
        Task<bool> RequestProductions();
    }
}