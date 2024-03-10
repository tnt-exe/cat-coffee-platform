using AutoMapper;
using BusinessObject.Model;
using CatCoffeePlatformAPI.Common;
using DAO.Helper;
using DTO.CoffeeShopDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using System.Globalization;

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
            return Ok(new ResponseBody<IEnumerable<CoffeeShopResponseDTO>>
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
            return Ok(new ResponseBody<CoffeeShopResponseDTO>
            {
                Title = "Get Successfully",
                Result = result.Payload
            });
        }

        [HttpGet]
        [Route("get-by-manager-id/{idManager}")]
        public async Task<IActionResult> GetCoffeeShopByManagerID(Guid idManager)
        {
            var result = await _coffeeShopRepo.GetByManagerID(idManager);

            if (result.IsError)
            {
                return NotFound();
            }
            return Ok(new ResponseBody<CoffeeShopResponseDTO>
            {
                Title = "Get Successfully",
                Result = result.Payload
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCoffeeShop(CoffeeShopCreate resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _coffeeShopRepo.Create(resource);
            if (result.IsError)
            {
                return BadRequest(new
                {
                    Title = "Create fail",
                });
            }
            return Ok(new ResponseBody<CoffeeShopResponseDTO>
            {
                Title = "Create Successfully",
                Result = result.Payload
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoffeeShop(CoffeeShopUpdate resource, int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _coffeeShopRepo.Update(resource, id);
                if (result.IsError)
                {
                    return NotFound();
                }

                return Ok(result.Payload);
            }
            return BadRequest(new ResponseBody<CoffeeShopResponseDTO>
            {
                Title = "Update Fail",
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoffeeShop(int id)
        {
            var result = await _coffeeShopRepo.Deleted(id);
            if (result.IsError)
            {
                return NotFound(new
                {
                    Title = "Delete Fail"
                });
            }
            return Ok(new
            {
                Title = "Delete successfully",
            });
        }

    }
}
