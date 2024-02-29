using DAO.UnitOfWork;
using BusinessObject.Enums;
using BusinessObject.Model;
using CatCoffeePlatformAPI.Permission;
using CatCoffeePlatformAPI.Service;
using DAO.Context;
using DAO.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Implement;
using Repository.Interface;
using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DTO.Common;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using DTO.BookingDTO;
using DTO.TimeFrameDTO;
using DTO.AreaDTO;

namespace CatCoffeePlatformAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var Configuration = builder.Configuration;

            // Add services to the container.

            // Add CORS
            builder.Services.AddCors(o =>
            {
                o.AddPolicy("All", p =>
                {
                    p.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });

            // Add this for Createing JWT Token
            builder.Services.AddSingleton<IKeyManager, KeyManager>();
            builder.Services.AddScoped<IJWTTokenService, JWTTokenService>();

            // Add this for authorization
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            builder.Services.AddScoped<IAuthorizationHandler, HasScopeHandler>();

            // Add this to only use UserManager<User> -> IUserStore<User>
            builder.Services.AddScoped<IUserStore<User>, CustomUserStore>();
            builder.Services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                // Passord settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                //UserName settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";


                options.Tokens.EmailConfirmationTokenProvider = "UserTokenProvider";
                options.Tokens.ChangeEmailTokenProvider = "UserTokenProvider";
                options.Tokens.PasswordResetTokenProvider = "UserTokenProvider";
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddCookie("cookie")
            .AddJwtBearer(x =>
            {
                var keyManager = new KeyManager();
                var key = keyManager.RsaKey;

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(key ?? throw new ArgumentException("Key not found for authentication scheme"))
                };
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

            builder.Services.AddControllers(config =>
            {
                using (var serviceProvider = builder.Services.BuildServiceProvider()) {
                    var readerFactory = serviceProvider.GetRequiredService<IHttpRequestStreamReaderFactory>();
                    config.ModelBinderProviders.Insert(0, new ModelBinderProvider(config.InputFormatters, readerFactory));
                }
            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errorStrings = new List<string>();
                        var modelState = actionContext.ModelState;

                        // values is IEnumerable of ModelStateEntry, we need to take error messages from each ModleStateEntry
                        // error messages of each ModelStateEntry is stored in Errors property, which will return Collection of ModelError
                        var values = modelState.Values;

                        foreach (var modelStateEntry in values)
                        {
                            foreach (var modelError in modelStateEntry.Errors)
                            {
                                errorStrings.Add(modelError.ErrorMessage);
                            }
                        }
                        return new BadRequestObjectResult(new
                        {
                            Status = 400,
                            Title = "One or more validation errors occurred",
                            Errors = errorStrings
                        });
                    };
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
                })
                .AddNewtonsoftJson(option =>
                    option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddOData(options => options
                    .AddRouteComponents("odata", GetEdmModel())
                    .Count()
                    .Filter()
                    .Expand()
                    .Select()
                    .OrderBy()
                    .SetMaxTop(null));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeShopPlatformAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddRepositories();
            
            // Add Repository to the container
            builder.Services.AddScoped<UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    // Customize the response for 401 Unauthorized status code
                    var responseBody = new
                    {
                        StatusCode = context.HttpContext.Response.StatusCode,
                        Title = "Unauthorize: You do not have permission to access this resource."
                    };
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(responseBody));
                }

                if (context.HttpContext.Response.StatusCode == 403)
                {
                    // Customize the response for 403 Forbidden status code
                    var responseBody = new
                    {
                        StatusCode = context.HttpContext.Response.StatusCode,
                        Title = "Forbidden: You do not have sufficient privileges to access this resource."
                    };
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(responseBody));
                }
            });

            app.UseHttpsRedirection();

            app.UseCors("All");

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<AreaDto>("Areas");
            builder.EntitySet<TimeFrameDto>("TimeFrames");
            builder.EntitySet<BookingResponseDTO>("Booking");

            // Defining the composite key
            /*var entityConfig = builder.EntitySet<BookingDTO>("Bookings").EntityType;
            entityConfig.HasKey(c => new { c.KeyProperty1, c.KeyProperty2 });*/

            return builder.GetEdmModel();
        }
    }
}