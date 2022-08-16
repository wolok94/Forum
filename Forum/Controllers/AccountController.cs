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
        [Route("account/register")]
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
        [Route("account/{accountId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task <IActionResult> GetById([FromRoute] int accountId)
        {
            var user = await accountService.GetById(accountId);
            return Ok (user);
        }
    }
}
