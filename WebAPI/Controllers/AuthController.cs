using Business.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenHandler _tokenHadler;

        public AuthController(IAuthService authService, ITokenHandler tokenHadler)
        {
            _authService = authService;
            _tokenHadler = tokenHadler;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterAuthDto authDto)
        {
            var result = await _authService.Register(authDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UserLogin(UserLoginDto authDto)
        {
            var result = await _authService.UserLogin(authDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CustomerUserLogin(CustomerLoginDto request)
        {
            var result = await _authService.CustomerLogin(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
