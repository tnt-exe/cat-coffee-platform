using DAO.Helper;
using DTO.BookingDTO;

namespace Repository.Interface
{
    public interface IBookingRepo
    {
        Task<OperationResult<BookingResponseDTO>> Create(BookingDTO resource);
        Task<OperationResult<IEnumerable<BookingResponseDTO>>> Get(int startPage, int endPage, int? quantity, int? slots, DateOnly? date, decimal? totalMoney, int? status, int? areaId, int? timeFrameId, Guid? userId, int? coffeeShopId);
    }
}
