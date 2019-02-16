using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Helper
{
    public class GetProducts : IGetProducts
    {
        private IStoreCatalogRepository _storeCatalogRepository;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public GetProducts(IStoreCatalogRepository repository, IMapper mapper, IConfiguration configuration)
        { 
            _storeCatalogRepository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<bool> RequestProducts()
        {
            var url = _configuration.GetSection("Apis:Products").Get<string>();
            var storeName = _configuration.GetSection("Store:Name").Get<string>();

            var client = new HttpClient();

            var response = await client.GetAsync(new Uri($"{url}?storeName={storeName}"));
            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();

                var produtos = JsonConvert.DeserializeObject<List<ProductToGet>>(responseText);

                foreach (var produto in produtos)
                {
                    var productToGet = _mapper.Map<ProductToGet>(produto);

                    _storeCatalogRepository.UpsertProduct(productToGet);
                }

                return true;
            }

            return false;
        }
    }
}
