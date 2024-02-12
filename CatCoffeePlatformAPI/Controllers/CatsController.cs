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
        [ProducesResponseType(typeof(IEnumerable<CatDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCats()
        {
            var result = await _catRepo.GetCats();
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(result.Payload);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CatDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCatById(int id)
        {
            var result = await _catRepo.GetCatById(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(result.Payload);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CatUpdate), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCat(int id, CatUpdate cat)
        {
            if (id != cat.CatId)
            {
                ModelState.AddModelError("CatId", "CatId is not valid");
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
        [ProducesResponseType(typeof(CatCreate), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCat(CatCreate cat)
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
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCat(int id)
        {
            var result = await _catRepo.DeleteCat(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : NoContent();
        }
    }
}
