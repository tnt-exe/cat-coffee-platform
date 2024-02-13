using CatCoffeePlatformAPI.Common;
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
        [ProducesResponseType(typeof(ResponseBody<IEnumerable<CatDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCats()
        {
            var result = await _catRepo.GetCats();
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<IEnumerable<CatDto>>()
                {
                    Title = "Get cat list success",
                    Result = result.Payload
                });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseBody<CatDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCatById(int id)
        {
            var result = await _catRepo.GetCatById(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<CatDto>
                {
                    Title = "Get cat success",
                    Result = result.Payload
                });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseBody<CatUpdate>), 200)]
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
                : Ok(new ResponseBody<CatUpdate>
                {
                    Title = "Update cat success",
                    Result = result.Payload
                });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseBody<CatCreate>), 200)]
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
                : Ok(new ResponseBody<CatCreate>
                {
                    Title = "Create cat success",
                    Result = result.Payload
                });
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
