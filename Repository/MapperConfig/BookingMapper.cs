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
                .ForMember(dst => dst.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")));
        }
    }
}
