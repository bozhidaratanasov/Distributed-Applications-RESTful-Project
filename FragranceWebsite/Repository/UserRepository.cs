using FragranceWebsite.Repository.IRepository;
using FragranceWebsite.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FragranceWebsite.Repository
{
    public class UserRepository : Repository<UserVM>, IUserRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public UserRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<UserVM> LoginAsync(string url, UserVM user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content = new StringContent
                    (JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
                return new UserVM();

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserVM>(jsonString);
            }
            else
                return new UserVM();
        }

        public async Task<bool> RegisterAsync(string url, UserVM user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content = new StringContent
                    (JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
                return false;

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
                return false;
        }
    }
}
