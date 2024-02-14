using AutoMapper;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.CatDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implement
{
    public class CatRepo : ICatRepo
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public CatRepo(IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork();
        }

        public async Task<OperationResult<CatCreate>> CreateCat(CatCreate catCreate)
        {
            var result = new OperationResult<CatCreate>
            {
                IsError = false
            };

            try
            {
                var catEntity = _mapper.Map<Cat>(catCreate);
                catEntity.IsDeleted = false;
                await _unitOfWork.CatDAO.Insert(catEntity);
                await _unitOfWork.SaveAsync();

                result.Payload = _mapper.Map<CatCreate>(catEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<object>> DeleteCat(int id)
        {
            var result = new OperationResult<object>
            {
                IsError = false
            };

            try
            {
                var cat = await _unitOfWork.CatDAO.GetByIDAsync(id);
                if (cat is null || cat.IsDeleted)
                {
                    result.AddError(ErrorCode.NotFound, "No cat found");
                    return result;
                }

                cat!.IsDeleted = true;
                _unitOfWork.CatDAO.Update(cat);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<CatDto>> GetCatById(int id)
        {
            var result = new OperationResult<CatDto>
            {
                IsError = false
            };

            string[] includeProperties = { nameof(Area), nameof(CoffeeShop) };
            var cat = await _unitOfWork.CatDAO
                .Get(filter: catEntity => catEntity.CatId == id && !catEntity.IsDeleted,
                    includeProperties: includeProperties)
                .FirstOrDefaultAsync();

            if (cat is null)
            {
                result.AddError(ErrorCode.NotFound, "No cat found");
                return result;
            }
            var catDto = _mapper.Map<CatDto>(cat);
            result.Payload = catDto;

            return result;
        }

        public async Task<OperationResult<IEnumerable<CatDto>>> GetCats()
        {
            var result = new OperationResult<IEnumerable<CatDto>>
            {
                IsError = false
            };

            string[] includeProperties = { nameof(Area), nameof(CoffeeShop) };
            var catListQueryable = await _unitOfWork.CatDAO
                .Get(filter: catEntity => !catEntity.IsDeleted,
                    includeProperties: includeProperties)
                .ToListAsync();
            var catList = _mapper.Map<IEnumerable<CatDto>>(catListQueryable);

            if (catList is null || !catList.Any())
            {
                result.AddError(ErrorCode.NotFound, "No cat found");
                return result;
            }

            result.Payload = catList;

            return result;
        }

        public async Task<OperationResult<CatUpdate>> UpdateCat(CatUpdate catUpdate)
        {
            var result = new OperationResult<CatUpdate>
            {
                IsError = false
            };

            try
            {
                var catEntity = await _unitOfWork.CatDAO
                    .Get(c => c.CatId == catUpdate.CatId
                        && !c.IsDeleted)
                    .FirstOrDefaultAsync();

                if (catEntity is null)
                {
                    result.AddError(ErrorCode.NotFound, "No cat found");
                    return result;
                }

                _mapper.Map(catUpdate, catEntity);

                _unitOfWork.CatDAO.Update(catEntity);
                await _unitOfWork.SaveAsync();

                result.Payload = _mapper.Map<CatUpdate>(catEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }

            return result;
        }
    }
}
