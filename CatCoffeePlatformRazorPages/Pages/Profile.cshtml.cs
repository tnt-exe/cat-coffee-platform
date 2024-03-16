using DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Client.Pages.Profile;

public class ProfileModel : PageModel
{
    private readonly HttpClient client = null!;
    private string UserInformationApiUrl = "";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileModel(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        client.DefaultRequestHeaders.Accept.Add(contentType);
        UserInformationApiUrl = "https://localhost:7039/api/User";
    }

    [BindProperty]
    public UserResponseDTO UserInformation { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            ViewData["warning"] = "Can not load user information";
            return Page();
        }

        var token = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"{UserInformationApiUrl}/{userId}");
        if (response.IsSuccessStatusCode)
        {
            string responseMessage = await response.Content.ReadAsStringAsync();
            var data = (JObject?)JsonConvert.DeserializeObject(responseMessage);
            if (data is null)
            {
                ViewData["warning"] = "Something wrong happends (Can not load data)";
                return Page();
            }
            var userInformation = data?["result"]?.ToObject<UserResponseDTO>();
            if (userInformation is null)
            {
                ViewData["warning"] = "Something wrong happends (Can not load data)";
                return Page();
            }
            UserInformation = userInformation;
            return Page();
        }
        else
        {
            ViewData["warning"] = "Can not load information";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var updateUser = new UserDTO
            {
                FirstName = UserInformation.FirstName,
                LastName = UserInformation.LastName,
                Email = UserInformation.Email,
                PhoneNumber = UserInformation.PhoneNumber,
            };
            string content = JsonSerializer.Serialize(updateUser);
            var contentData = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            var token = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PutAsync($"{UserInformationApiUrl}/{UserInformation.Id}", contentData);
            string responseMessage = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = (JObject)JsonConvert.DeserializeObject(responseMessage)!;
                TempData["Information"] = data?["title"]?.Value<string>();
            }
            else
            {
                var data = (JObject)JsonConvert.DeserializeObject(responseMessage)!;
                ViewData["warning"] = data?["errors"]?.Value<List<string>>()?.FirstOrDefault() ?? "Error";
                return Page();
            }

            return RedirectToPage("./home");
        }

        ViewData["warning"] = "Invalid input";
        return Page();
    }
}
