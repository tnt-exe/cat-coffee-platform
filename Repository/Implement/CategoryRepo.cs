using AutoMapper;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implement
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryRepo(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult<CategoryCreate>> CreateCategory(CategoryCreate categoryCreate)
        {
            var result = new OperationResult<CategoryCreate>
            {
                IsError = false
            };
            try
            {
                var categoryEntity = _mapper.Map<Category>(categoryCreate);
                await _unitOfWork.CategoryDAO.Insert(categoryEntity);
                await _unitOfWork.SaveAsync();
                result.Payload = _mapper.Map<CategoryCreate>(categoryEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var result = new OperationResult<IEnumerable<CategoryDto>>
            {
                IsError = false
            };

            try
            {
                var categories = await _unitOfWork.CategoryDAO
                    .Get().ToListAsync();

                var categoriesList = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                if (categoriesList is null || !categoriesList.Any())
                {
                    result.AddError(ErrorCode.NotFound, "No category found");
                    return result;
                }

                result.Payload = categoriesList;

                return result;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<CategoryDto>> GetCategoryById(int id)
        {
            var result = new OperationResult<CategoryDto>
            {
                IsError = false
            };

            var category = await _unitOfWork.CategoryDAO
                .Get(filter: categoryEntity => categoryEntity.CategoryId == id)
                .FirstOrDefaultAsync();

            if (category is null)
            {
                result.AddError(ErrorCode.NotFound, "No category found");
                return result;
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            result.Payload = categoryDto;

            return result;
        }

        public async Task<OperationResult<CategoryUpdate>> UpdateCategory(CategoryUpdate categoryUpdate)
        {
            var result = new OperationResult<CategoryUpdate>
            {
                IsError = false
            };
            try
            {
                var categoryEntity = _mapper.Map<Category>(categoryUpdate);
                _unitOfWork.CategoryDAO.Update(categoryEntity);

                await _unitOfWork.SaveAsync();
                result.Payload = _mapper.Map<CategoryUpdate>(categoryEntity);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }

            return result;
        }
    }
}