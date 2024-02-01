using BusinessObject.Model;
using Microsoft.Extensions.DependencyInjection;
using Repository.Implement;
using Repository.Interface;
using Repository.MapperConfig;

namespace Repository
{
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

            services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
            return services;
        }
    }
}
