using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;

namespace FragranceAPI.UserAuth
{
    public interface IUserRepository
    {
        bool IsUnique(string username);

        User Authenticate(string username, string password);

        User Register(string username, string password, string email);
    }
}
