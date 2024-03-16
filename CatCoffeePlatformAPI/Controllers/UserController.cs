using DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO resource, Guid id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.Update(resource, id);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Update failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                return Ok(new
                {
                    Title = "Update successfully",
                    Result = result.Payload
                });
            }

            return BadRequest(new
            {
                Title = "Update failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.Delete(id);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Delete failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                return Ok(new
                {
                    Title = "Delete successfully",
                });
            }

            return BadRequest(new
            {
                Title = "Delete failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.GetById(id);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Get failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                return Ok(new
                {
                    Result = result.Payload
                });
            }

            return BadRequest(new
            {
                Title = "Get failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }

        [HttpGet]
        [Authorize(Policy = "Staff Manager Admin")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int startPage,
            [FromQuery] int endPage,
            [FromQuery] int? quantity,
            [FromQuery] string? name,
            [FromQuery] byte? role,
            [FromQuery] byte? status,
            [FromQuery] int? coffeeShopId,
            [FromQuery] int? managedShopId)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.Get(startPage, endPage, quantity, name, role, status, coffeeShopId, managedShopId);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Get failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                return Ok(new
                {
                    Result = result.Payload
                });
            }

            return BadRequest(new
            {
                Title = "Get failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }
    }
}
