using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.AreaDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implement
{
    public class AreaRepo : IAreaRepo
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public AreaRepo(IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork();
        }

        public async Task<OperationResult<AreaCreate>> CreateArea(AreaCreate areaCreate)
        {
            var result = new OperationResult<AreaCreate>
            {
                IsError = false
            };

            try
            {
                var areaEntity = _mapper.Map<Area>(areaCreate);
                await _unitOfWork.AreaDAO.Insert(areaEntity);
                await _unitOfWork.SaveAsync();

                result.Payload = _mapper.Map<AreaCreate>(areaEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<AreaDto>> GetAreaById(int id)
        {
            var result = new OperationResult<AreaDto>
            {
                IsError = false
            };

            string[] includeProperties = { nameof(CoffeeShop) };
            var area = await _unitOfWork.AreaDAO
                .Get(filter: areaEntity => areaEntity.AreaId == id,
                    includeProperties: includeProperties)
                .FirstOrDefaultAsync();

            if (area is null)
            {
                result.AddError(ErrorCode.NotFound, "No area found");
                return result;
            }
            var areaDto = _mapper.Map<AreaDto>(area);
            result.Payload = areaDto;

            return result;
        }

        public async Task<OperationResult<IEnumerable<AreaDto>>> GetAreas()
        {
            var result = new OperationResult<IEnumerable<AreaDto>>
            {
                IsError = false
            };

            string[] includeProperties = { nameof(CoffeeShop) };
            var areaListQueryable = await _unitOfWork.AreaDAO
                .Get(includeProperties: includeProperties)
                .ToListAsync();

            var areaList = _mapper.Map<IEnumerable<AreaDto>>(areaListQueryable);

            if (areaList is null || !areaList.Any())
            {
                result.AddError(ErrorCode.NotFound, "No area found");
                return result;
            }

            result.Payload = areaList;

            return result;
        }

        public async Task<OperationResult<AreaUpdate>> UpdateArea(AreaUpdate areaUpdate)
        {
            var result = new OperationResult<AreaUpdate>
            {
                IsError = false
            };

            try
            {
                var areaEntity = await _unitOfWork.AreaDAO
                    .Get(areaEntity => areaEntity.AreaId == areaUpdate.AreaId)
                    .FirstOrDefaultAsync();

                if (areaEntity is null)
                {
                    result.AddError(ErrorCode.NotFound, "No area found");
                }

                _mapper.Map(areaUpdate, areaEntity);

                _unitOfWork.AreaDAO.Update(areaEntity!);
                await _unitOfWork.SaveAsync();

                result.Payload = _mapper.Map<AreaUpdate>(areaEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }

            return result;
        }

        public IQueryable<AreaDto> GetDbSet()
        {
            var query = _unitOfWork.AreaDAO.GetDbSet();
            return query.ProjectTo<AreaDto>(_mapper.ConfigurationProvider);
        }
    }
}
