using Data.Entities;
using FragranceAPI.UserAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User model)
        {
            bool isUsernameUnique = _userRepo.IsUnique(model.Username);
            if (!isUsernameUnique)
                return BadRequest(new { message = "User already exists" });

            var user = _userRepo.Register(model.Username, model.Password, model.Email);

            if (user == null)
                return BadRequest(new { messsage = "Error while registering" });

            return Ok();
        }


    }
}
