using BusinessObject.Model;
using CatCoffeePlatformAPI.Controllers.Base;
using DTO.CategoryDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? categoryId,
            [FromQuery]string? categoryName, 
            [FromQuery] int? pageIndex = 0,
            [FromQuery] int? pageSize = 10)
        {
            var response = await _categoryRepo.GetAll(x =>
                (categoryId == null || x.CategoryId == categoryId) ||
                (categoryName == null || x.CategoryName.Contains(categoryName)), pageIndex, pageSize);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
        }

        [HttpPost]
            public async Task<IActionResult> Create([FromBody] CategoryCreate category)
            {
                var response = await _categoryRepo.Create(category);
                return response.IsError
                    ? HandleErrorResponse(response.Errors)
                    : Created("/api/category", response.Payload);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var response = await _categoryRepo.GetById(id);
                return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] Category category)
            {
                var response = await _categoryRepo.Update(id, category);
                return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var response = await _categoryRepo.Delete(id);
                return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
            }
        }
    }