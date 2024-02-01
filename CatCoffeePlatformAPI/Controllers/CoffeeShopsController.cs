using BusinessObject.Model;
using DAO.Helper;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeShopsController : ControllerBase
    {
        private readonly ICoffeeShopRepo _coffeeShopRepo;
        public CoffeeShopsController(ICoffeeShopRepo coffeeShopRepo)
        {
            _coffeeShopRepo = coffeeShopRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCoffeeShops()
        {
            var result = await _coffeeShopRepo.GetAllCoffeeShops();
            if (result.IsError)
            {
                return NotFound();
            }
            return Ok(new
            {
                Title = "Get Successfully",
                Result = result.Payload
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoffeeShopByID(int id)
        {
            var result = await _coffeeShopRepo.GetByID(id);
            if (result.IsError)
            {
                return NotFound();
            }
            return Ok(new
            {
                Title = "Get Successfully",
                Result = result.Payload
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCoffeeShop(CoffeeShopCreate resource)
        {

            var result = await _coffeeShopRepo.Create(resource);
            if (result.IsError)
            {
                return BadRequest();
            }
            return Ok(new
            {
                Title = "Create Successfully",
                Result = result.Payload
            });

        }

    }
}
