using FragranceWebsite.Repository.IRepository;
using FragranceWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FragranceWebsite.Repository
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public SaleRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
