using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.AreaDTO;
using DTO.TimeFrameDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implement
{
    public class TimeFrameRepo : ITimeFrameRepo
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public TimeFrameRepo(IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork();
        }

        public async Task<OperationResult<TimeFrameCreate>> CreateTimeFrame(TimeFrameCreate timeFrameCreate)
        {
            var result = new OperationResult<TimeFrameCreate>
            {
                IsError = false
            };

            try
            {
                var timeFrameEntity = _mapper.Map<TimeFrame>(timeFrameCreate);
                await _unitOfWork.TimeFrameDAO.Insert(timeFrameEntity);
                await _unitOfWork.SaveAsync();

                result.Payload = _mapper.Map<TimeFrameCreate>(timeFrameEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }

            return result;
        }

        public async Task<OperationResult<TimeFrameDto>> GetTimeFrameById(int id)
        {
            var result = new OperationResult<TimeFrameDto>
            {
                IsError = false
            };

            string[] includeProperty = { nameof(CoffeeShop) };
            var timeFrame = await _unitOfWork.TimeFrameDAO
                .Get(filter: timeFrameEntity => timeFrameEntity.TimeFrameId == id,
                    includeProperties: includeProperty)
                .FirstOrDefaultAsync();

            if (timeFrame is null)
            {
                result.AddError(ErrorCode.NotFound, "No time frame found");
                return result;
            }
            var timeFrameDto = _mapper.Map<TimeFrameDto>(timeFrame);
            result.Payload = timeFrameDto;

            return result;
        }

        public async Task<OperationResult<IEnumerable<TimeFrameDto>>> GetTimeFrames()
        {
            var result = new OperationResult<IEnumerable<TimeFrameDto>>
            {
                IsError = false
            };

            string[] includeProperty = { nameof(CoffeeShop) };
            var timeFramesQueryable = await _unitOfWork.TimeFrameDAO
                .Get(includeProperties: includeProperty)
                .ToListAsync();

            var timeFrameList = _mapper.Map<IEnumerable<TimeFrameDto>>(timeFramesQueryable);

            if (timeFrameList is null || !timeFrameList.Any())
            {
                result.AddError(ErrorCode.NotFound, "No time frame found");
                return result;
            }

            result.Payload = timeFrameList;

            return result;
        }

        public async Task<OperationResult<TimeFrameUpdate>> UpdateTimeFrame(TimeFrameUpdate timeFrameUpdate)
        {
            var result = new OperationResult<TimeFrameUpdate>
            {
                IsError = false
            };

            try
            {
                var timeFrameEntity = await _unitOfWork.TimeFrameDAO
                    .Get(timeFrameEntity => timeFrameEntity.TimeFrameId == timeFrameUpdate.TimeFrameId)
                    .FirstOrDefaultAsync();

                if (timeFrameEntity is null)
                {
                    result.AddError(ErrorCode.NotFound, "No time frame found");
                }

                _mapper.Map(timeFrameUpdate, timeFrameEntity);

                _unitOfWork.TimeFrameDAO.Update(timeFrameEntity!);
                await _unitOfWork.SaveAsync();

                result.Payload = _mapper.Map<TimeFrameUpdate>(timeFrameEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }

            return result;
        }

        public IQueryable<TimeFrameDto> GetDbSet()
        {
            var query = _unitOfWork.TimeFrameDAO.GetDbSet();
            return query.ProjectTo<TimeFrameDto>(_mapper.ConfigurationProvider);
        }
    }
}
