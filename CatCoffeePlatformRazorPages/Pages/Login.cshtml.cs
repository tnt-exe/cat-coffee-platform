using DTO.UserDTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using CatCoffeePlatformRazorPages.Common;
using BusinessObject.Enums;

namespace CatCoffeePlatformRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient client = null!;
        private string LoginApiUrl = "";
        private IHttpContextAccessor httpContextAccessor;

        public LoginModel(IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            LoginApiUrl = "https://localhost:7039/api/auth/login";
            this.httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public UserLogin UserLogin { get; set; } = default!;

        public IActionResult OnGet()
        {
            var isAuthenticated = httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier) ?? false;
            if (isAuthenticated)
            {
                TempData["Information"] = "Already login";
                return RedirectToPage("./home");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                string content = JsonSerializer.Serialize(UserLogin);
                var contentData = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(LoginApiUrl, contentData);
                string responseMessage = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<ResponseBody<UserResponseDTO>>(responseMessage, options);
                    if (data is null)
                    {
                        ViewData["warning"] = "Something wrong happends (Can not load data)";
                        return Page();
                    }

                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, data.Result?.Id.ToString() ?? "Undefined"));
                    userClaims.Add(new Claim("First Name", data.Result?.FirstName ?? "Undefined"));
                    userClaims.Add(new Claim("Last Name", data.Result?.LastName ?? "Undefined"));
                    userClaims.Add(new Claim("scope", data.Result?.Role.ToString() ?? "Undefined"));

                    var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

                    await HttpContext.SignInAsync(userPrincipal);
                    HttpContext.Response.Cookies.Append("activeNavItem", "home");

                    TempData["Information"] = "Login Successfully";
                    return RedirectToPage("./home");
                }
                else
                {
                    var data = (JObject)JsonConvert.DeserializeObject(responseMessage)!;
                    ViewData["warning"] = data?["title"]?.Value<string>() ?? "Warning";
                    ViewData["errors"] = data?["errors"]?.ToObject<List<string>>();
                    return Page();
                }
            }
            else
            {
                ViewData["warning"] = "Invalid input";
                return Page();
            }
        }
    }
}