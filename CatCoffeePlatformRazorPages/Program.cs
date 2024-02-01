using BusinessObject.Enums;
using CatCoffeePlatformAPI.Permission;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "Prn231-cookie";
        config.LoginPath = "/login";
        config.LogoutPath = "/logout";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.Requirements.Add(new HasScopeRequirement(((int)Role.Administrator).ToString(), Configuration["Jwt:Issuer"]!));
    });

    options.AddPolicy("Customer", policy =>
    {
        policy.Requirements.Add(new HasScopeRequirement(((int)Role.Customer).ToString(), Configuration["Jwt:Issuer"]!));
    });

    options.AddPolicy("Staff", policy =>
    {
        policy.Requirements.Add(new HasScopeRequirement(((int)Role.Staff).ToString(), Configuration["Jwt:Issuer"]!));
    });

    options.AddPolicy("Manager", policy =>
    {
        policy.Requirements.Add(new HasScopeRequirement(((int)Role.Manager).ToString(), Configuration["Jwt:Issuer"]!));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(e =>
{
    e.MapRazorPages();

    e.MapPost("/logout", (HttpContext context) =>
    {
        context.Response.Cookies.Delete("Prn231-cookie");
        context.Response.Redirect("/");
    });
});

app.Run();
