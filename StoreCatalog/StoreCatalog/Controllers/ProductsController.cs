using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


//teste
namespace GeekBurger.StoreCatalog.Controllers
{
    /// <summary>
    /// Retorna todos produtos disponíveis para o cliente 
    /// aplicando suas restrições
    /// </summary>
    /// <param name="query">Classe que pede o Nome da Loja, Id do Usuário e Restrições</param>
    /// <returns>Lista de Produtos Disponíveis para Produção</returns>
    [Produces("application/json")]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private IStoreCatalogRepository _storeCatalogRepository;
        private IMapper _mapper;

        public ProductsController(IStoreCatalogRepository repository, IMapper mapper)
        {
            _storeCatalogRepository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var produtos = _storeCatalogRepository.GetProducts();

            var ProductByStoreToGet = _mapper.Map<IEnumerable<ProductByStoreToGet>>(produtos);

            return Ok(ProductByStoreToGet);
        }

        //[HttpGet]
        //public IActionResult GetProductsByStore(QueryProductByStore query)
        //{
        //    var produtos = _storeCatalogRepository.GetProducts();

        //    var ProductByStoreToGet = _mapper.Map<IEnumerable<ProductByStoreToGet>>(produtos);

        //    return Ok(ProductByStoreToGet);
        //}
    }
}