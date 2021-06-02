using FragranceWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite.Repository.IRepository
{
    public interface IUserRepository : IRepository<UserVM>
    {
        Task<UserVM> LoginAsync(string url, UserVM user);

        Task<bool> RegisterAsync(string url, UserVM user);
    }
}
