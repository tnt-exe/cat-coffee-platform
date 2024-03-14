using CatCoffeePlatformRazorPages.Common;
using DTO.BookingDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Grpc.Net.Client;


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



        #region get
        public IEnumerable<BookingResponseDTO> Bookings { get; set; } = new List<BookingResponseDTO>();
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
        #endregion


        #region gRPC
        /*public IEnumerable<BookingObject> Bookings { get; set; } = new List<BookingObject>();
        public IActionResult OnGet()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7039");
            var client = new BookingCRUD.BookingCRUDClient(channel);

            Bookingss bookings = client.SelectAll(new Empty());

            Bookings = bookings.Items;

            return Page();
        }*/
        #endregion
    }
}
