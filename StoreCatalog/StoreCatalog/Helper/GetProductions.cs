using AutoMapper;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeekBurger.Production.Contract;

namespace GeekBurger.StoreCatalog.Helper
{
    public class GetProductions : IGetProductions
    {
        private IStoreCatalogRepository _storeCatalogRepository;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public GetProductions(IStoreCatalogRepository repository, IMapper mapper, IConfiguration configuration)
        {
            _storeCatalogRepository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<bool> RequestProductions()
        {
            var url = _configuration.GetSection("Apis:Products").Get<string>();
            var storeName = _configuration.GetSection("Store:Name").Get<string>();

            var client = new HttpClient();

            var response = await client.GetAsync(new Uri($"{url}?storeName={storeName}"));
            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();

                var productions = JsonConvert.DeserializeObject<List<ProductionToGet>>(responseText);

                foreach (var production in productions)
                {
                    _storeCatalogRepository.UpsertProduction(production);
                }

                return true;
            }

            return false;
        }
    }
}
