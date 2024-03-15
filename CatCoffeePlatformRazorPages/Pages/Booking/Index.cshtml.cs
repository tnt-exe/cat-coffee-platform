using CatCoffeePlatformRazorPages.Common;
using DTO.BookingDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Grpc.Net.Client;
using BusinessObject.Enums;
using BusinessObject.Model;

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

        public bool ShowCustomer { get; set; } = false;

        #region get
        public IEnumerable<BookingResponseDTO> Bookings { get; set; } = new List<BookingResponseDTO>();
        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }

            var role = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "scope")?.Value;

            if(role is null)
            {
                ViewData["warning"] = "Get user information failed";
                return Page();
            }

            IEnumerable<BookingResponseDTO>? bookingList = null;
            if (role.Equals(((int)Role.Customer).ToString()))
            {
                var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "00000000-0000-0000-0000-000000000000";
                var apiResponse = await _apiArea
                    .GetQueryAsync<ResponseBody<IEnumerable<BookingResponseDTO>>>("userId=" + userId);
                bookingList = apiResponse!.Result;
            }
            else if (role.Equals(((int)Role.Staff).ToString()))
            {
                ShowCustomer = true;
                var shopId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "coffeeshop id")?.Value;
                if(shopId != null)
                {
                    var apiResponse = await _apiArea
                        .GetQueryAsync<ResponseBody<IEnumerable<BookingResponseDTO>>>("coffeeShopId=" + shopId);
                    bookingList = apiResponse!.Result;
                }
            }
            else if (role.Equals(((int)Role.Manager).ToString()))
            {
                ShowCustomer = true;
                var shopId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "managed coffeeshop id")?.Value;
                var apiResponse = await _apiArea
                    .GetQueryAsync<ResponseBody<IEnumerable<BookingResponseDTO>>>("coffeeShopId=" + shopId);
                bookingList = apiResponse!.Result;
            }
            else if (role.Equals(((int)Role.Administrator).ToString()))
            {
                ShowCustomer = true;
                var apiResponse = await _apiArea
                    .GetAsync<ResponseBody<IEnumerable<BookingResponseDTO>>>();
                bookingList = apiResponse!.Result;
            }

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
