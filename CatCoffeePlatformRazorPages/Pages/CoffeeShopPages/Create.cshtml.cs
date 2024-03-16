using CatCoffeePlatformRazorPages.Common;
using DTO.CoffeeShopDTO;
using DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CatCoffeePlatformRazorPages.Pages.CoffeeShopPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiHelper _apiShop;
        private string ApiUrl = "";
        private readonly HttpClient client = default!;
        private IHttpContextAccessor _httpContextAccessor;
        public CreateModel(IHttpContextAccessor httpContextAccessor)
        {
            _apiShop = new ApiHelper(ApiResources.CoffeeShops);
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _httpContextAccessor = httpContextAccessor;
            ApiUrl = "https://localhost:7039/api/User";
        }

        [BindProperty]
        public CoffeeShopCreate CoffeeShop { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            var token = _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == "token").FirstOrDefault()?.Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage _response = await client.GetAsync(ApiUrl);
            string _strData = await _response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var response = JsonSerializer.Deserialize<ResponseBody<List<UserResponseDTO>>>(_strData, options)!;
            var userList = response.Result;
            ViewData["Email"] = new SelectList(userList, "Id", "Email");
            return Page();
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var newShop = new CoffeeShopCreate
            {
                ShopName = CoffeeShop.ShopName,
                Address = CoffeeShop.Address,
                OpeningTime = CoffeeShop.OpeningTime,
                ClosingTime = CoffeeShop.ClosingTime,
                ContactNumber = CoffeeShop.ContactNumber,
                Email = CoffeeShop.Email,
                Description = CoffeeShop.Description,
                ManagerId = CoffeeShop.ManagerId,
            };
            var result = await _apiShop.PostAsync(newShop);

            if (result)
            {
                TempData["shop-msg"] = "Create Shop Success";
                return RedirectToPage("./Index");
            }
            TempData["shop-msg"] = "Create Shop Fail";
            return Page();
        }
    }
}
