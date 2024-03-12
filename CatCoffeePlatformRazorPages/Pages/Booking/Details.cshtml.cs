using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;
using DTO.BookingDTO;
using CatCoffeePlatformRazorPages.Common;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _bookingApi;

        public DetailsModel()
        {
            _bookingApi = new ApiHelper("bookings");
        }

        public BookingResponseDTO Booking { get; set; } = default!;
        public bool IsSuccess { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id is null)
            {
                ViewData["warning"] = "Booking not found";
                return Page();
            }
            var apiResponse = await _bookingApi.GetAsync<ResponseBody<BookingResponseDTO>>(id.ToString()!);
            if(apiResponse?.Result is null)
            {
                ViewData["warning"] = "Booking not found";
                return Page();
            }
            Booking = apiResponse.Result;
            return Page();
        }
    }
}
