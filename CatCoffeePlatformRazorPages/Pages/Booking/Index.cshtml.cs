﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CatCoffeePlatformRazorPages.Common;
using DTO.BookingDTO;
using BusinessObject.Model;
using DTO.AreaDTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiArea;
        private IHttpContextAccessor httpContextAccessor;

        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            _apiArea = new ApiHelper(ApiResources.Bookings);
            this.httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<BookingResponseDTO> Bookings { get;set; } = new List<BookingResponseDTO>();

        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }

            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "00000000-0000-0000-0000-000000000000";

            var apiResponse = await _apiArea
                .GetQueryAsync<ResponseBody<IEnumerable<BookingResponseDTO>>>("userId=" + userId);
            var bookingList = apiResponse!.Result;

            if (bookingList is not null)
            {
                Bookings = bookingList;
            }

            return Page();
        }
    }
}
