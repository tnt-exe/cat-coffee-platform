using CatCoffeePlatformAPI.Service;
using DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IJWTTokenService _jwtTokenService;

        public AuthController(IUserRepo userRepo, IJWTTokenService jwtTokenService)
        {
            _userRepo = userRepo;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLogin resource)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.Login(resource);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Login failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                if (result.Payload is null)
                {
                    return BadRequest(new
                    {
                        Title = "Login failed",
                        Errors = new string[1] { "Payload is null" }
                    });
                }
                var tokenString = _jwtTokenService.CreateToken(result.Payload);
                return Ok(new
                {
                    Title = "Login successfully",
                    Token = tokenString,
                    Result = result.Payload
                });
            }

            return BadRequest(new
            {
                Title = "Login failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDTO resource)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.Register(resource);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Register failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                return Ok(new
                {
                    Title = "Register successfully",
                    Result = result.Payload
                });
            }

            return BadRequest(new
            {
                Title = "Register failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }
    }
}
