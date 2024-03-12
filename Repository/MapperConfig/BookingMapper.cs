using AutoMapper;
using BusinessObject.Model;
using DTO.BookingDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void BookingMapper()
        {
            CreateMap<BookingDTO, Booking>();
            CreateMap<BookingDTO_BookingProduct, BookingProduct>();

            CreateMap<Booking, BookingResponseDTO>()
                .ForMember(dest => dest.CoffeeShop, opt => opt.MapFrom(src => src.CoffeeShop))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.TimeFrame, opt => opt.MapFrom(src => src.TimeFrame))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.BookingProducts));
            CreateMap<CoffeeShop, BookingResponseDTO_CoffeeShop>();
            CreateMap<Area, BookingResponseDTO_Area>();
            CreateMap<TimeFrame, BookingResponseDTO_TimeFrame>();
            CreateMap<User, BookingResponseDTO_User>();
            CreateMap<BookingProduct, BookingResponseDTO_Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product!.Price))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Product!.Unit));
        }
    }
}
