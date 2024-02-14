using AutoMapper;
using BusinessObject.Enums;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.BookingDTO;
using DTO.UserDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository.Implement
{
    public class BookingRepo : IBookingRepo
    {
        private readonly UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IValidateGet _validateGet;

        public BookingRepo(UnitOfWork unitOfWork, IMapper mapper, IValidateGet validateGet)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validateGet = validateGet;
        }

        public async Task<OperationResult<BookingResponseDTO>> Create(BookingDTO resource) 
        {
            var result = new OperationResult<BookingResponseDTO>()
            {
                IsError = false
            };

            if(resource.AreaId is null || resource.TimeFrameId is null || resource.UserId is null || resource.CoffeeShopId is null)
            {
                result.AddError(ErrorCode.BadRequest, "Invalid input");
                return result;
            }

            if(resource.BookingProducts.Count() > 0)
            {
                foreach(var product in resource.BookingProducts)
                {
                    if(product.Quantity is null || product.ProductId is null)
                    {
                        result.AddError(ErrorCode.BadRequest, "Invalid input");
                        return result;
                    }
                }
            }

            if(resource.Slots is null || resource.Slots == 0)
            {
                result.AddError(ErrorCode.BadRequest, "Please choose the number of slots");
                return result;
            }

            if (resource.Date is null)
            {
                result.AddError(ErrorCode.BadRequest, "Please choose date to book");
                return result;
            }

            var maxSlot = await _unitOfWork.AreaDAO.Get(a => a.AreaId == resource.AreaId).Select(a => a.MaxSlots).FirstOrDefaultAsync();

            var bookedSlots = _unitOfWork.BookingDAO.Get(b => b.AreaId == resource.AreaId && b.TimeFrameId == resource.TimeFrameId && b.Date == resource.Date).Sum(b => b.Slots);

            if((bookedSlots + resource.Slots) > maxSlot)
            {
                result.AddError(ErrorCode.BadRequest, $"Only {maxSlot - bookedSlots} slots are available");
                return result;
            }

            try
            {
                Booking newBooking = _mapper.Map<Booking>(resource);
                newBooking.BookingDate = DateTime.UtcNow;
                await _unitOfWork.BookingDAO.Insert(newBooking);
                var addResult = await _unitOfWork.SaveChangesAsync();
                if (addResult)
                {
                    result.Payload = _mapper.Map<BookingResponseDTO>(newBooking);
                    return result;
                }
                else
                {
                    result.AddError(ErrorCode.BadRequest, "Create booking failed");
                    return result;
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message;
                if(innerException != null && innerException.Contains("conflicted with the FOREIGN KEY constraint"))
                {
                    if (innerException.Contains(nameof(Booking.AreaId)))
                    {
                        result.AddError(ErrorCode.BadRequest, "Area not found");
                    }
                    if (innerException.Contains(nameof(Booking.TimeFrameId)))
                    {
                        result.AddError(ErrorCode.BadRequest, "TimeFrame not found");
                    }
                    if (innerException.Contains(nameof(Booking.UserId)))
                    {
                        result.AddError(ErrorCode.BadRequest, "User not found");
                    }
                    if (innerException.Contains(nameof(Booking.CoffeeShopId)))
                    {
                        result.AddError(ErrorCode.BadRequest, "CoffeeShop not found");
                    }
                }
                else
                {
                    result.AddError(ErrorCode.ServerError, ex.Message);
                    result.AddError(ErrorCode.ServerError, innerException ?? "");
                }
                return result;
            }
            catch (OperationCanceledException ex)
            {
                result.AddError(ErrorCode.ServerError, "The operation has been cancelled");
                result.AddError(ErrorCode.ServerError, ex.Message);
                return result;
            }
        }

        public async Task<OperationResult<IEnumerable<BookingResponseDTO>>> Get(int startPage, int endPage, int? quantity, int? slots, DateOnly? date, decimal? totalMoney, int? status, int? areaId, int? timeFrameId, Guid? userId, int? coffeeShopId)
        {
            var result = new OperationResult<IEnumerable<BookingResponseDTO>>()
            {
                IsError = false
            };

            int quantityResult = 0;
            _validateGet.ValidateGetRequest(ref startPage, ref endPage, quantity, ref quantityResult);
            if (quantityResult == 0)
            {
                result.AddError(ErrorCode.BadRequest, "Invalid get quantity");
                return result;
            }

            var expressions = new List<Expression>();
            ParameterExpression pe = Expression.Parameter(typeof(Booking), "r");
            MethodInfo? containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            if (containsMethod is null)
            {
                result.AddError(ErrorCode.ServerError, "Method Contains can not found from string type");
                return result;
            }

            expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.Deleted)), Expression.Constant(false)));

            if(slots is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.Slots)), Expression.Constant(slots)));
            }

            if(date is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.Date)), Expression.Constant(date)));
            }

            if(totalMoney is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.TotalMoney)), Expression.Constant(totalMoney)));
            }

            if(status is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.Status)), Expression.Constant(status)));
            }

            if(areaId is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.AreaId)), Expression.Constant(areaId)));
            }

            if(timeFrameId is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.TimeFrameId)), Expression.Constant(timeFrameId)));
            }

            if(userId is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.UserId)), Expression.Constant(userId)));
            }

            if(coffeeShopId is not null)
            {
                expressions.Add(Expression.Equal(Expression.Property(pe, nameof(Booking.CoffeeShopId)), Expression.Constant(coffeeShopId)));
            }

            Expression combined = expressions.Aggregate((accumulate, next) => Expression.AndAlso(accumulate, next));
            Expression<Func<Booking, bool>> where = Expression.Lambda<Func<Booking, bool>>(combined, pe);

            var bookings = await _unitOfWork.BookingDAO
                .Get(where)
                .AsNoTracking()
                .Skip((startPage - 1) * quantityResult)
                .Take((endPage - startPage + 1) * quantityResult)
                .ToArrayAsync();

            result.Payload = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);
            return result;
        }
    }
}
