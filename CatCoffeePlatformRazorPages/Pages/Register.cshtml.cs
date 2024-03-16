using BusinessObject.Enums;
using DTO.UserDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace CatCoffeePlatformRazorPages.Register;

public class RegisterModel : PageModel
{
    private readonly HttpClient client = null!;
    private string ProductApiUrl = "";
    private IHttpContextAccessor httpContextAccessor;

    public RegisterModel(IHttpContextAccessor httpContextAccessor)
    {
        client = new HttpClient();
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        client.DefaultRequestHeaders.Accept.Add(contentType);
        ProductApiUrl = "https://localhost:7039/api/auth/register";
        this.httpContextAccessor = httpContextAccessor;
    }

    public IActionResult OnGet()
    {
        var checkRole = int.TryParse(httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "scope")?.Value, out int role);
        if (checkRole && role == (int)Role.Administrator)
        {
            IsAdmin = true;
        }
        else if (role == (int)Role.Manager)
        {
            IsManager = true;
        }
        else if (role == (int)Role.Staff)
        {
            IsStaff = true;
        }
        return Page();
    }

    [BindProperty]
    public UserDTO Account { get; set; } = default!;
    [BindProperty]
    public int CoffeeShopId { get; set; }
    [BindProperty]
    public bool WorkingStaff { get; set; } = false;

    public bool IsAdmin { get; set; } = false;
    public bool IsManager { get; set; } = false;
    public bool IsStaff { get; set; } = false;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || Account is null)
        {
            ViewData["warning"] = "Invalid input";
            return Page();
        }

        if (Account.Password != Account.ConfirmPassword)
        {
            ViewData["warning"] = "Password and confirm password does not match";
            return Page();
        }

        string strData = JsonSerializer.Serialize(Account);
        var contentData = new StringContent(strData, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(ProductApiUrl, contentData);
        string responseMessage = await response.Content.ReadAsStringAsync();
        var data = (JObject)JsonConvert.DeserializeObject(responseMessage)!;
        if (response.IsSuccessStatusCode)
        {
            TempData["Information"] = data?["title"]?.Value<string>();
            return RedirectToPage("./home");
        }
        else
        {
            ViewData["warning"] = data?["title"]?.Value<string>() ?? "Error";
            ViewData["errors"] = data?["errors"]?.ToObject<List<string>>();
            return Page();
        }
    }
}
