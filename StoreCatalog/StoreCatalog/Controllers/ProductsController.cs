﻿using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;


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
        private IHealthCheck _healthCheck;
        private IConfiguration _configuration;
        private IStoreCatalogRepository _storeCatalogRepository;

        public ProductsController(IConfiguration configuration, IHealthCheck healthCheck, IStoreCatalogRepository storeCatalogRepository)
        {
            _configuration = configuration;
            _healthCheck = healthCheck;
            _storeCatalogRepository = storeCatalogRepository;
        }

        [HttpGet]
        public IActionResult GetProducts(QueryProductByStore query)
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
    }
}