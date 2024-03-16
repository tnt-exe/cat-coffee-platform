using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CoffeeShopDTO;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class AreasModel : PageModel
    {
        private readonly ApiHelper _apiArea;
        private readonly ApiHelper _apiODataArea;
        private readonly ApiHelper _apiODataTimeFrame;
        private readonly ApiHelper _apiODataBooking;

        public AreasModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
            _apiODataArea = new ApiHelper($"{ApiResources.Areas}/odata");
            _apiODataTimeFrame = new ApiHelper($"{ApiResources.TimeFrames}/odata");
            _apiODataBooking = new ApiHelper($"booking");
        }

        public CoffeeShopResponseDTO? CoffeeShop { get; set; }
        [BindProperty]
        public string? CoffeeShopJson { get; set; }
        public IList<AreaDto> Areas { get; set; } = new List<AreaDto>();
        public IList<TimeFrameDto> TimeFrames { get; set; } = new List<TimeFrameDto>();
        public DateOnly BookedDate { get; set; }
        public int BookedTimeFrameId { get; set; }
        public string CountKeyResult { get; set; } = "@odata.count";

        public async Task<IActionResult> OnPostAsync()
        {
            if (CoffeeShopJson is null)
            {
                ViewData["warning"] = "Can not load data";
                ViewData["errors"] = new List<string>
                {
                    "Coffee Shop information not found"
                };
                return Page();
            }
            else
            {
                var shop = JsonSerializer.Deserialize<CoffeeShopResponseDTO>(CoffeeShopJson);
                if (shop is null)
                {
                    ViewData["warning"] = "Can not load data";
                    ViewData["errors"] = new List<string>
                    {
                        "Coffee Shop information not found"
                    };
                    return Page();
                }
                CoffeeShop = shop;
            }

            var currentDateTime = DateTime.UtcNow;
            var currentDate = DateOnly.FromDateTime(currentDateTime);
            BookedDate = currentDate;

            var timeFrameResponse = await _apiODataTimeFrame.GetODataAsync<IEnumerable<TimeFrameDto>>($"filter = coffeeShopId eq {CoffeeShop.CoffeeShopId}");
            TimeFrames = timeFrameResponse?.ToList() ?? new List<TimeFrameDto>();
            var firstTimeFrame = TimeFrames.FirstOrDefault();
            BookedTimeFrameId = firstTimeFrame?.TimeFrameId ?? 0;

            var areaResponse = await _apiODataArea.GetODataAsync<IEnumerable<AreaDto>>($"filter = coffeeShopId eq {CoffeeShop.CoffeeShopId}");
            var areas = areaResponse?.ToList() ?? new List<AreaDto>();

            if (TimeFrames.IsNullOrEmpty())
            {
                Areas = areas;
                ViewData["warning"] = "Shop does not have any available time frame";
                return Page();
            }

            foreach (var area in areas)
            {
                var responseMessage = await _apiODataBooking.GetODataAsync($"count=true&top=0&filter=Date eq {currentDate.ToString("yyyy-MM-dd")} and AreaId eq {area.AreaId} and TimeFrameId eq {firstTimeFrame?.TimeFrameId}");
                var data = (JObject)JsonConvert.DeserializeObject(responseMessage ?? "")!;
                var bookedSlots = data?["@odata.count"]?.Value<int>() ?? 0;
                var availableSlots = area.MaxSlots - bookedSlots;
                area.AvailableSlots = availableSlots < 0 ? 0 : availableSlots;
            }

            Areas = areas;

            return Page();
        }
    }
}
