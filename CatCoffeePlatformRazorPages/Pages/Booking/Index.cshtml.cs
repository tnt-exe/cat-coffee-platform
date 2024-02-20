using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CatCoffeePlatformRazorPages.Common;
using DTO.BookingDTO;
using BusinessObject.Model;
using DTO.AreaDTO;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class IndexModel : PageModel
    {
        private readonly ApiHelper _apiArea;

        public IndexModel()
        {
            _apiArea = new ApiHelper("booking");
        }

        public IEnumerable<BookingResponseDTO> Bookings { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var apiResponse = await _apiArea
                .GetAsync<ResponseBody<IEnumerable<BookingResponseDTO>>>();
            var bookingList = apiResponse!.Result;

            if (bookingList is not null)
            {
                Bookings = bookingList;
            }
        }
    }
}
