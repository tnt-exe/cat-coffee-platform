using DAO.Context;
using DAO.Helper;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Repository.Implement;
using Repository.Interface;
using Repository.MapperConfig;

namespace Repository;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped<IAreaRepo, AreaRepo>();
        services.AddScoped<IBillRepo, BillRepo>();
        services.AddScoped<IBookingProductRepo, BookingProductRepo>();
        services.AddScoped<IBookingRepo, BookingRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<ICatRepo, CatRepo>();
        services.AddScoped<ICoffeeShopRepo, CoffeeShopRepo>();
        services.AddScoped<IPaymentRepo, PaymentRepo>();
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<ITimeFrameRepo, TimeFrameRepo>();
        services.AddScoped<IUserRepo, UserRepo>();
        #endregion

        #region Helper
        services.AddScoped<IValidateGet, ValidateGet>();
        #endregion

        services.AddSingleton<IDesignTimeDbContextFactory<ApplicationDbContext>, DbContextFactory>();

        services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
        return services;
    }
}
