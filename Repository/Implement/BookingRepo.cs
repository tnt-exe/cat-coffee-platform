using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.BookingDTO;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Repository.Interface;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository.Implement
{
    public class BookingRepo : IBookingRepo
    {
        private readonly UnitOfWork _unitOfWork;

        // This mapper will use the configuration that we defined at AutoMapperConfig class, the class which is used by injected AutoMapper
        private IMapper _mapper;

        // This mapper is for other configuration stored in other class (for purpose of using ProjectTo)
        private IMapper _projectToMapper;

        private readonly IValidateGet _validateGet;

        public BookingRepo(UnitOfWork unitOfWork, IMapper mapper, IValidateGet validateGet)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validateGet = validateGet;

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BookingProjectToProfile());
            });
            _projectToMapper = new Mapper(config);
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
                .Include(b => b.User)
                .Include(b => b.CoffeeShop)
                .AsNoTracking()
                .Skip((startPage - 1) * quantityResult)
                .Take((endPage - startPage + 1) * quantityResult)
                .ToArrayAsync();

            result.Payload = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);
            return result;
        }

        public IQueryable<BookingResponseDTO> GetDbSet(ODataQueryOptions<BookingResponseDTO>? queryOptions)
        {
            var query = _unitOfWork.BookingDAO.GetDbSet();
            if (queryOptions is not null)
            {
                IQueryable result = query.ProjectTo<BookingResponseDTO>(_projectToMapper.ConfigurationProvider);

                var settings = new ODataQuerySettings();
                var filter = queryOptions.Filter;
                var orderBy = queryOptions.OrderBy;
                var skip = queryOptions.Skip;
                var top = queryOptions.Top;
                var selectExpand = queryOptions.SelectExpand;

                if (filter is not null)
                {
                    if (filter.RawValue.ToLower().Contains("date eq"))
                    {
                        var filterStrings = queryOptions.Filter.RawValue.Split(" and ");
                        var dateFilterString = filterStrings?.Where(f => f.ToLower().Contains("date eq")).FirstOrDefault()?.Replace("Date eq ", "").Trim();
                        if (DateOnly.TryParse(dateFilterString, out var date))
                        {
                            var pe = Expression.Parameter(typeof(BookingResponseDTO), "b");
                            var expression = Expression.Equal(Expression.Property(pe, nameof(BookingResponseDTO.Date)), Expression.Constant(date));
                            Expression<Func<BookingResponseDTO, bool>> where = Expression.Lambda<Func<BookingResponseDTO, bool>>(expression, pe);
                            result = result.Cast<BookingResponseDTO>().Where(where);
                        }
                        var acceptFilter = filterStrings?.Where(f => !f.ToLower().Contains("date eq")) ?? Enumerable.Empty<string>();
                        string? newFilterString = null;
                        if (acceptFilter.Count() == 1)
                        {
                            newFilterString = acceptFilter.First();
                        }
                        else if(acceptFilter.Count() > 1)
                        {
                            newFilterString = String.Join(" and ", acceptFilter);
                        }

                        if(newFilterString is not null)
                        {
                            var newFilter = new FilterQueryOption(newFilterString, queryOptions.Context,
                            new Microsoft.OData.UriParser.ODataQueryOptionParser(
                                model: queryOptions.Context.Model,
                                targetEdmType: queryOptions.Context.NavigationSource.EntityType(),
                                targetNavigationSource: queryOptions.Context.NavigationSource,
                                queryOptions: new Dictionary<string, string> { { "$filter", newFilterString } },
                                container: queryOptions.Context.RequestContainer
                        ));

                            result = newFilter.ApplyTo(result, settings);
                        }
                    }
                    else
                    {
                        result = filter.ApplyTo(result, settings);
                    }
                }

                if (queryOptions.Count?.Value == true)
                {
                    queryOptions.Request.ODataFeature().TotalCount = ((IQueryable<BookingResponseDTO>)result).Select(b => b.Slots).Sum();
                }
                if (orderBy != null)
                    result = orderBy.ApplyTo(result, settings);
                if (skip != null)
                    result = skip.ApplyTo(result, settings);
                if (top != null)
                    result = top.ApplyTo(result, settings);

                // This is used to change the existed values
                // add the NextLink if one exists
                /*if (queryOptions.Request.ODataFeature().NextLink != null)
                {
                    originalRequest.ODataFeature().NextLink = queryOptions.Request.ODataFeature().NextLink;
                }
                // add the TotalCount if one exists
                if (queryOptions.Request.ODataFeature().TotalCount != null)
                {
                    originalRequest.ODataFeature().TotalCount = queryOptions.Request.ODataFeature().TotalCount;
                }*/

                /*if (selectExpand != null)
                {
                    result = selectExpand.ApplyTo(result, settings);
                    return result.Cast<object>();
                }
                else
                {
                    return result.Cast<BookingResponseDTO>();
                }*/

                return result.Cast<BookingResponseDTO>();
            }


            return query.ProjectTo<BookingResponseDTO>(_projectToMapper.ConfigurationProvider);
        }

        private class BookingProjectToProfile : Profile
        {
            public BookingProjectToProfile()
            {
                CreateMap<Booking, BookingResponseDTO>()
                    .ForMember(dest => dest.Area, opt => opt.Ignore())
                    .ForMember(dest => dest.TimeFrame, opt => opt.Ignore())
                    .ForMember(dest => dest.User, opt => opt.Ignore())
                    .ForMember(dest => dest.CoffeeShop, opt => opt.Ignore());
            }
        }
    }
}
