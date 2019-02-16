using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Contract;
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
        [HttpGet]
        public IActionResult GetProductsByStore(QueryProductByStore query)
        {
            var a = new ProductByStoreToGet();

            return Ok(a);
        }
    }
}