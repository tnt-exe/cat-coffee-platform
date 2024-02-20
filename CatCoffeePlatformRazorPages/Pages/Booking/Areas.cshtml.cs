using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;
using DAO.Context;
using DTO.CoffeeShopDTO;
using System.Text.Json;
using DTO.AreaDTO;
using CatCoffeePlatformRazorPages.Common;
using CatCoffeePlatformAPI.Common;
using DTO.TimeFrameDTO;
using Microsoft.IdentityModel.Tokens;

namespace CatCoffeePlatformRazorPages.Pages.Booking
{
    public class AreasModel : PageModel
    {
        private readonly ApiHelper _apiArea;
        private readonly ApiHelper _apiODataArea;
        private readonly ApiHelper _apiODataTimeFrame;

        public AreasModel()
        {
            _apiArea = new ApiHelper(ApiResources.Areas);
            _apiODataArea = new ApiHelper($"{ApiResources.Areas}/odata");
            _apiODataTimeFrame = new ApiHelper($"{ApiResources.TimeFrames}/odata");
        }

        public CoffeeShopResponseDTO? CoffeeShop { get; set; }
        [BindProperty]
        public string? CoffeeShopJson { get; set; }
        public IList<AreaDto> Areas { get;set; } = new List<AreaDto>();
        public IList<TimeFrameDto> TimeFrames { get; set; } = new List<TimeFrameDto>();

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

            var timeFrameResponse = await _apiODataTimeFrame.GetODataAsync<IEnumerable<TimeFrameDto>>($"filter = coffeeShopId eq {CoffeeShop.CoffeeShopId}");
            TimeFrames = timeFrameResponse?.ToList() ?? new List<TimeFrameDto>();
            var firstTimeFrameId = TimeFrames.FirstOrDefault()?.TimeFrameId;

            var areaResponse = await _apiODataArea.GetODataAsync<IEnumerable<AreaDto>>($"filter = coffeeShopId eq {CoffeeShop.CoffeeShopId}");
            var areas = areaResponse?.ToList() ?? new List<AreaDto>();

            if (TimeFrames.IsNullOrEmpty())
            {
                Areas = areas;
                ViewData["warning"] = "Shop does not have any available time frame";
                return Page();
            }

            foreach(var area in areas)
            {

            }

            return Page();
        }
    }
}
