using CatCoffeePlatformAPI.Common.Odata;
using DTO.BookingDTO;
using DTO.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepo _bookingRepo;
    public BookingsController(IBookingRepo bookingRepo)
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _bookingRepo.GetById(id);
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

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _bookingRepo.Delete(id);
            if (result.IsError)
            {
                return BadRequest(new
                {
                    Title = "Delete failed",
                    Errors = result.Errors.Select(e => e.Message)
                });
            }

            return Ok(new
            {
                Title = "Delete successfully",
            });
        }

        return BadRequest(new
        {
            Title = "Delete failed",
            Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
        });
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateBooking([FromBody] BookingDTO resource, int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _bookingRepo.Update(resource, id);
            if (result.IsError)
            {
                return BadRequest(new
                {
                    Title = "Update failed",
                    Errors = result.Errors.Select(e => e.Message)
                });
            }

            return Ok(new
            {
                Title = "Update successfully",
                Result = result.Payload
            });
        }

        return BadRequest(new
        {
            Title = "Update failed",
            Errors = ModelState.Values.SelectMany(m => m.Errors.Select(e => e.ErrorMessage))
        });
    }
}

public class BookingController : ODataController
{
    private readonly IBookingRepo _bookingRepo;
    public BookingController(IBookingRepo bookingRepo)
    {
        _bookingRepo = bookingRepo;
    }

    [BookingEnableQueryAttribute]
    public IActionResult Get(ODataQueryOptions<BookingResponseDTO>? options)
    {
        var result = _bookingRepo.GetDbSet(options);
        if (options?.SelectExpand is not null)
        {
            return Ok(options.SelectExpand.ApplyTo(result, new ODataQuerySettings()));
        }
        return Ok(result);
    }
}