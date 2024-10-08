﻿using BusinessObject.Model;
using CatCoffeePlatformRazorPages.Common;
using DTO.AreaDTO;
using DTO.CatDTO;
using DTO.CoffeeShopDTO;
using DTO.TimeFrameDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApiHelper _apiShop;
        private readonly ApiHelper _apiCat;
        private readonly ApiHelper _apiProduct;
        private readonly ApiHelper _apiTime;
        private readonly ApiHelper _apiArea;

        public DetailsModel()
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
            _apiCat = new ApiHelper(ApiResources.Cats);
            _apiProduct = new ApiHelper(ApiResources.Products);
            _apiTime = new ApiHelper(ApiResources.TimeFrames);
            _apiArea = new ApiHelper(ApiResources.Areas);
        }

        public CoffeeShopResponseDTO CoffeeShop { get; set; } = default!;
        public IEnumerable<CatDto> Cat { get; set; } = default!;
        public IEnumerable<AreaDto> Area { get; set; } = default!;
        public IEnumerable<TimeFrameDto> TimeFrame { get; set; } = default!;
        public IEnumerable<Product> Product { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var apiResponse = await _apiShop.GetAsync<ResponseBody<CoffeeShopResponseDTO>>($"{id}");
            var shop = apiResponse!.Result;

            var apiCatResponse = await _apiCat.GetAsync<ResponseBody<IEnumerable<CatDto>>>();
            var catList = apiCatResponse.Result.Where(c => c.CoffeeShopId == id);

            var apiAreaResponse = await _apiArea.GetAsync<ResponseBody<IEnumerable<AreaDto>>>();
            var areaList = apiAreaResponse!.Result.Where(c => c.CoffeeShopId == id);

            var apiTimeResponse = await _apiTime.GetAsync<ResponseBody<IEnumerable<TimeFrameDto>>>();
            var timeFrameList = apiTimeResponse!.Result.Where(c => c.CoffeeShopId == id);

            var apiProductResponse = await _apiProduct.GetQueryAsync<IEnumerable<Product>>($"shopId={id}&pageIndex=0&pageSize=10&includeProperties=Category");


            if (shop == null || catList == null)
            {
                return NotFound();
            }
            else
            {
                CoffeeShop = shop;
                Cat = catList;
                Area = areaList;
                TimeFrame = timeFrameList;
                Product = apiProductResponse!;
            }
            return Page();
        }


    }
}
