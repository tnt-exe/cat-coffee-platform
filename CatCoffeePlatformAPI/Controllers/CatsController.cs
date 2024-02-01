using BusinessObject.Model;
using DTO.CatDTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly ICatRepo _catRepo;

        public CatsController(ICatRepo catRepo)
        {
            _catRepo = catRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatDto>>> GetCats()
        {
            var result = await _catRepo.GetCats();
            if (result.IsError)
            {
                return NotFound();
            }

            return Ok(result.Payload);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CatDto>> GetCat(int id)
        {
            var result = await _catRepo.GetCatById(id);
            if (result.IsError)
            {
                return NotFound();
            }

            return Ok(result.Payload);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCat(int id, CatUpdate cat)
        {
            if (id != cat.CatId)
            {
                return BadRequest("Invalid id");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _catRepo.UpdateCat(cat);
            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Payload);
        }

        [HttpPost]
        public async Task<ActionResult<Cat>> PostCat(CatCreate cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _catRepo.CreateCat(cat);
            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Payload);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCat(int id)
        {
            var result = await _catRepo.DeleteCat(id);
            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
