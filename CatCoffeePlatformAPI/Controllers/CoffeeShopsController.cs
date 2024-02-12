﻿using BusinessObject.Model;
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var checkEmail = _coffeeShopRepo.CheckEmail(resource.Email ?? "");
            if (checkEmail != null)
            {
                return BadRequest(new
                {
                    Titile = "Create fail",
                    Errors = "Email Existed"
                });
            }
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

        [HttpPut]
        public async Task<IActionResult> UpdateCoffeeShop(CoffeeShopUpdate resource, int id)
        {
            if (ModelState.IsValid)
            {
                var checkEmail = _coffeeShopRepo.CheckEmail(resource.Email ?? "");

                var result = await _coffeeShopRepo.Update(resource, id);
                if (result.IsError)
                {
                    return NotFound(new
                    {
                        Titile = "Update Fail",
                        Errors = "Shop not found"
                    });
                }

                return Ok(new
                {
                    Title = "Update Successfully",
                    Result = result.Payload
                });
            }
            return BadRequest(new
            {
                Title = "Update Fail",
            });
        }

        [HttpDelete]
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