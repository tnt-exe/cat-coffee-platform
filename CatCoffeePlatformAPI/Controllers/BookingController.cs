using BusinessObject.Enums;
using DTO.BookingDTO;
using DTO.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Implement;
using Repository.Interface;
using System.Xml.Linq;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepo _bookingRepo;
        public BookingController(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings(
            [FromQuery] int startPage,
            [FromQuery] int endPage,
            [FromQuery] int? quantity,
            [FromQuery] int? slots,
            [FromQuery][ModelBinder(BinderType = typeof(DateOnlyModelBinder))] DateOnly? date,
            [FromQuery] decimal? totalMoney,
            [FromQuery] int? status,
            [FromQuery] int? areaId,
            [FromQuery] int? timeFrameId,
            [FromQuery] Guid? userId,
            [FromQuery] int? coffeeShopId)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookingRepo.Get(startPage, endPage, quantity, slots, date, totalMoney, status, areaId, timeFrameId, userId, coffeeShopId);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Get failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                return Ok(new
                {
                    Result = result.Payload
                });
            }

            return BadRequest(new
            {
                Title = "Get failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO resource)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookingRepo.Create(resource);
                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        Title = "Create failed",
                        Errors = result.Errors.Select(e => e.Message)
                    });
                }

                return Ok(new
                {
                    Result = result.Payload
                });
            }

            return BadRequest(new
            {
                Title = "Create failed",
                Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
            });
        }
    }
}
