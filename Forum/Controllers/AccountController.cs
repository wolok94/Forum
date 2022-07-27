using Forum.Models;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("api/account/register")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([FromBody] CreateUserDto userDto)
        {
            accountService.RegisterUser(userDto);
            return Ok();
        }

        [HttpPost]
        [Route("api/account/login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            var token = accountService.GenerateJWT(dto);
            return Ok(token);

        }
        [HttpGet]
        [Route("api/accounts")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAll()
        {
            var users = accountService.GetAll();
            return Ok(users);
        }

    }
}
