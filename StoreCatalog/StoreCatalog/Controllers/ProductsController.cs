using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using GeekBurger.StoreCatalog.Service.UserWithLessOffer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace GeekBurger.StoreCatalog.Controllers
{
    public class ProductsController : Controller
    {
        private IHealthCheck _healthCheck;
        private IConfiguration _configuration;
        private IStoreCatalogRepository _storeCatalogRepository;
        private IUserWithLessOffer _userWithLessOffer;

        public ProductsController(IConfiguration configuration, IHealthCheck healthCheck, IStoreCatalogRepository storeCatalogRepository, IUserWithLessOffer userWithLessOffer)
        {
            _configuration = configuration;
            _healthCheck = healthCheck;
            _storeCatalogRepository = storeCatalogRepository;
            _userWithLessOffer = userWithLessOffer;
        }

        /// <summary>
        /// Retorna todos produtos disponíveis para o cliente 
        /// aplicando suas restrições
        /// </summary>
        /// <param name="query">Classe que pede o Nome da Loja, Id do Usuário e Restrições</param>
        /// <returns>Lista de Produtos Disponíveis para Produção</returns>
        [Produces("application/json")]
        [Route("api/products")]
        [HttpGet]
        public IActionResult GetProducts(QueryProductByStore query)
        {
            if (_healthCheck.Healthy)
            {
                var areas = _storeCatalogRepository.GetAreas();
                var filteredProducts = _storeCatalogRepository.GetProductsByRestrictions(query.Restrictions);

                //any area allowed for this user
                var allowedAreas = areas.Where(area =>
                    query.Restrictions.All(restriction => area.Restrictions.Any(c => c.Name.Contains(restriction))));

                //products that can be produced in allowed areas for this user
                var allowedProducts = filteredProducts.Where(product => product
                    //all ingredients from this product are not in the restriction list of at least one allowed area
                    .Ingredients.All(ingredient => allowedAreas.Any(area => !area.Restrictions.Any(c => c.Name.Contains(ingredient.Name)))));

                // Envia a mensagem UserWithLessOffer se o usuário tiver menos de 4 resultados
                if (allowedProducts.Count() < 4)
                    _userWithLessOffer.SendUserWithLessOffer(query.UserId, query.Restrictions);

                return Ok(allowedProducts.Select(p => new ProductByStoreToGet
                {
                    ProductId = p.ProductId,
                    Image = p.Image,
                    Items = p.Ingredients.Select(c => new ItemToGet { ItemId = c.ItemId, Name = c.Name }).ToList(),
                    Name = p.Name,
                    Price = p.Price,
                    StoreId = p.StoreId
                }));
            }
            else
                return StatusCode(503);
        }
    }
}