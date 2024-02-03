using AutoMapper;
using DAO.UnitOfWork;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            AreaMapper();
            BillMapper();
            BookingProductMapper();
            BookingMapper();
            CategoryMapper();
            CatMapper();
            CoffeeShopMapper();
            PaymentMapper();
            ProductMapper();
            TimeFrameMapper();
            UserMapper();
        }
    }
}
