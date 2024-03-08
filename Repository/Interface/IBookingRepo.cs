using DAO.Helper;
using DTO.BookingDTO;
using Microsoft.AspNetCore.OData.Query;

namespace Repository.Interface
{
    public interface IBookingRepo
    {
        Task<OperationResult<BookingResponseDTO>> Create(BookingDTO resource);
        Task<OperationResult<IEnumerable<BookingResponseDTO>>> Get(int startPage, int endPage, int? quantity, int? slots, DateOnly? date, decimal? totalMoney, int? status, int? areaId, int? timeFrameId, Guid? userId, int? coffeeShopId);
        IQueryable<BookingResponseDTO> GetDbSet(ODataQueryOptions<BookingResponseDTO>? options);
        Task<OperationResult<BookingResponseDTO>> GetById(int id);
        Task<OperationResult<object>> Delete(int id);
        Task<OperationResult<BookingResponseDTO>> Update(BookingDTO resource, int id);
    }
}
