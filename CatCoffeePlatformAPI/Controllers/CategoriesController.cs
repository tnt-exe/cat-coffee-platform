using CatCoffeePlatformAPI.Common;
using CatCoffeePlatformAPI.Controllers.Base;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoriesController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseBody<IEnumerable<CategoryDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryRepo.GetCategories();
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<IEnumerable<CategoryDto>>
                {
                    Title = "Get category list success",
                    Result = result.Payload
                });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseBody<CategoryDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryRepo.GetCategoryById(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<CategoryDto>
                {
                    Title = "Get category success",
                    Result = result.Payload
                });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseBody<CategoryCreate>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreate categoryCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _categoryRepo.CreateCategory(categoryCreate);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<CategoryCreate>
                {
                    Title = "Create category success",
                    Result = result.Payload
                });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseBody<CategoryUpdate>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdate categoryUpdate)
        {
            if (id != categoryUpdate.CategoryId)
            {
                ModelState.AddModelError("CategoryId", "CategoryId in body and path is not match");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _categoryRepo.UpdateCategory(categoryUpdate);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<CategoryUpdate>
                {
                    Title = "Update category success",
                    Result = result.Payload
                });
        }
    }
}