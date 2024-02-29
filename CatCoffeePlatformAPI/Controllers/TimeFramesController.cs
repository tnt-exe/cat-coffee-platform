using CatCoffeePlatformAPI.Common;
using CatCoffeePlatformAPI.Controllers.Base;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeFramesController : BaseController
    {
        private readonly ITimeFrameRepo _timeFrameRepo;
        public TimeFramesController(ITimeFrameRepo timeFrameRepo)
        {
            _timeFrameRepo = timeFrameRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseBody<IEnumerable<TimeFrameDto>>), 200)]
        [ProducesResponseType(404)]
        [ODataIgnored]
        public async Task<IActionResult> GetTimeFrames()
        {
            var result = await _timeFrameRepo.GetTimeFrames();
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<IEnumerable<TimeFrameDto>>
                {
                    Title = "Get timeframe list success",
                    Result = result.Payload
                });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseBody<TimeFrameDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTimeFrameById(int id)
        {
            var result = await _timeFrameRepo.GetTimeFrameById(id);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<TimeFrameDto>
                {
                    Title = "Get timeframe success",
                    Result = result.Payload
                });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseBody<TimeFrameCreate>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTimeFrame(TimeFrameCreate timeFrame)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _timeFrameRepo.CreateTimeFrame(timeFrame);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<TimeFrameCreate>
                {
                    Title = "Create timeframe success",
                    Result = result.Payload
                });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseBody<TimeFrameUpdate>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTimeFrame(int id, TimeFrameUpdate timeFrame)
        {
            if (id != timeFrame.TimeFrameId)
            {
                ModelState.AddModelError("TimeFrameId", "TimeFrameId is not valid");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _timeFrameRepo.UpdateTimeFrame(timeFrame);
            return result.IsError
                ? HandleErrorResponse(result.Errors)
                : Ok(new ResponseBody<TimeFrameUpdate>
                {
                    Title = "Update timeframe success",
                    Result = result.Payload
                });
        }

        [HttpGet("odata")]
        [EnableQuery]
        public IActionResult GetAreasOData()
        {
            return Ok(_timeFrameRepo.GetDbSet());
        }
    }
}
