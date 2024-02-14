using CatCoffeePlatformAPI.Common;
using CatCoffeePlatformAPI.Controllers.Base;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : BaseController
    {
        private readonly IAreaRepo _areaRepo;
        public AreasController(IAreaRepo areaRepo)
        {
            _areaRepo = areaRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseBody<IEnumerable<AreaDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAreas()
        {
            var result = await _areaRepo.GetAreas();
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<IEnumerable<AreaDto>>
                {
                    Title = "Get area list success",
                    Result = result.Payload
                });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseBody<AreaDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAreaById(int id)
        {
            var result = await _areaRepo.GetAreaById(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<AreaDto>
                {
                    Title = "Get area success",
                    Result = result.Payload
                });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseBody<AreaCreate>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateArea(AreaCreate area)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _areaRepo.CreateArea(area);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<AreaCreate>
                {
                    Title = "Create area success",
                    Result = result.Payload
                });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseBody<AreaUpdate>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateArea(int id, AreaUpdate area)
        {
            if (id != area.AreaId)
            {
                ModelState.AddModelError("AreaId", "AreaId is not valid");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _areaRepo.UpdateArea(area);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<AreaUpdate>
                {
                    Title = "Update area success",
                    Result = result.Payload
                });
        }
    }
}
