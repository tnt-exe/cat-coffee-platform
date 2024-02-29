using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Model;
using DTO.Common;
using DTO.CoffeeShopDTO;
using DTO.AreaDTO;
using DTO.TimeFrameDTO;
using Microsoft.IdentityModel.Tokens;
using CatCoffeePlatformRazorPages.Common;
using System.Text.Json;
using System.Net.Http.Headers;
using DTO.UserDTO;
using System.Security.Claims;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class ProductsModel : PageModel
    {
        private readonly HttpClient client = null!;
        private readonly ApiHelper _apiProduct;
        private readonly ApiHelper _apiTimeFrame;
        private string ProductUrl = "";
        private IHttpContextAccessor httpContextAccessor;

        public ProductsModel(IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiProduct = new ApiHelper("product");
            _apiTimeFrame = new ApiHelper(ApiResources.TimeFrames);
            ProductUrl = "https://localhost:7039/api/Product";
            this.httpContextAccessor = httpContextAccessor;
        }

        public CoffeeShopResponseDTO? CoffeeShop { get; set; }
        public AreaDto? Area { get; set; }
        public TimeFrameDto? TimeFrame { get; set; }

        [BindProperty]
        public string? CoffeeShopJson { get; set; }
        [BindProperty]
        public string? AreaJson { get; set; }
        [BindProperty]
        public int? TimeFrameId { get; set; }
        [BindProperty]
        [ModelBinder(BinderType = typeof(DateOnlyModelBinder))]
        public DateOnly? BookedDate { get; set; }
        [BindProperty]
        public int? BookedSlots { get; set; } = 0;
        public decimal TotalRentalPrice { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = "";

        public IList<Product> Products { get;set; } = new List<Product>();


        public async Task<IActionResult> OnPostAsync()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (!isAuthenticated)
            {
                return RedirectToPage("../login");
            }
            else
            {
                var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "00000000-0000-0000-0000-000000000000";
                UserId = new Guid(userId);
                Token = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "token")?.Value ?? "";
            }

            var errors = new List<string>();
            if(CoffeeShopJson is null)
            {
                errors.Add("Coffee Shop information not found");
            }
            if(AreaJson is null)
            {
                errors.Add("Area information not found");
            }
            if(TimeFrameId is null)
            {
                errors.Add("Time frame not found");
            }
            if(BookedDate is null)
            {
                errors.Add("Booked date not found");
            }
            if (BookedSlots is null)
            {
                errors.Add("Booked slots not found");
            }
            if (!errors.IsNullOrEmpty())
            {
                ViewData["warning"] = "Can not load data";
                ViewData["errors"] = errors;
                return Page();
            }

            var shop = JsonSerializer.Deserialize<CoffeeShopResponseDTO>(CoffeeShopJson ?? "");
            var area = JsonSerializer.Deserialize<AreaDto>(AreaJson ?? "");
            var timeFrameResponse = await _apiTimeFrame.GetAsync<ResponseBody<TimeFrameDto>>(TimeFrameId.ToString() ?? "");
            var timeFrame = timeFrameResponse?.Result;
            IList<Product> products = new List<Product>();

            var productsResponse = await client.GetAsync($"{ProductUrl}?shopId={shop?.CoffeeShopId ?? 0}");
            string productsResponseMessage = await productsResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (productsResponse.IsSuccessStatusCode)
            {
                products = JsonSerializer.Deserialize<List<Product>>(productsResponseMessage, options) ?? Enumerable.Empty<Product>().ToList();
            }

            if(shop is null || area is null || timeFrame is null)
            {
                ViewData["warning"] = "Load data failed";
                return Page();
            }

            if(!TimeOnly.TryParse(timeFrame.StartTime, out var startTime)
                || !TimeOnly.TryParse(timeFrame.EndTime, out var endTime))
            {
                ViewData["warning"] = "Load bill failed";
                return Page();
            }
            else
            {
                var bookedDuration = (endTime - startTime).TotalMinutes;
                var pricePerHour = area.PricePerHour;
                var test = (double)pricePerHour * (bookedDuration / 60);
                if (decimal.TryParse(test.ToString(), out var totalRentalPrice))
                {
                    TotalRentalPrice = totalRentalPrice;
                }
                else
                {
                    ViewData["warning"] = "Load bill failed";
                    return Page();
                }
            }

            CoffeeShop = shop;
            Area = area;
            TimeFrame = timeFrame;
            Products = products;

            return Page();
        }
    }
}
