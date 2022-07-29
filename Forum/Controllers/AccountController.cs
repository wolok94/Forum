using Forum.Models;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    
    [ApiController]
    [Route("api/")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("/account/register")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Create([FromBody] CreateUserDto userDto)
        {
            await accountService.RegisterUser(userDto);
            return Ok();
        }

        [HttpPost]
        [Route("account/login")]
        public async Task <IActionResult> Login([FromBody] LoginDto dto)
        {
            return Ok(await accountService.GenerateJWT(dto));
        }
        [HttpGet]
        [Route("accounts")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> GetAll()
        {
            return Ok(await accountService.GetAll());
        }

    }
}
