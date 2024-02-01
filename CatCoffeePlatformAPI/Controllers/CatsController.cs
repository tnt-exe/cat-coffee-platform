using CatCoffeePlatformAPI.Controllers.Base;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : BaseController
    {
        private readonly ICatRepo _catRepo;

        public CatsController(ICatRepo catRepo)
        {
            _catRepo = catRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCats()
        {
            var result = await _catRepo.GetCats();
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(result.Payload);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCat(int id)
        {
            var result = await _catRepo.GetCatById(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(result.Payload);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCat(int id, CatUpdate cat)
        {
            if (id != cat.CatId)
            {
                ModelState.AddModelError("CatId", "CatId is not match");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _catRepo.UpdateCat(cat);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(result.Payload);
        }

        [HttpPost]
        public async Task<IActionResult> PostCat(CatCreate cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _catRepo.CreateCat(cat);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(result.Payload);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCat(int id)
        {
            var result = await _catRepo.DeleteCat(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(result.Payload);
        }
    }
}
