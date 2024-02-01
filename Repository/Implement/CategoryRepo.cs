using System.Linq.Expressions;
using AutoMapper;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.CategoryDTO;
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
        
        public Task<OperationResult<IEnumerable<Category>>> GetAll(Expression<Func<Category, bool>>? filter, Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null, string[]? includeProperties = null)
        {
            var result = new OperationResult<IEnumerable<Category>>();
            try
            {
                var categories = _unitOfWork.CategoryDAO.Get();
                if (filter != null)
                {
                    categories = categories.Where(filter);
                }

                if (categories.ToList().Count == 0)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Does not exist category");
                }
                else
                {
                    result.IsError = false;
                    result.Payload = categories;
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.AddUnknownError(e.Message);
            }
            return Task.FromResult(result);
        }

        public async Task<OperationResult<Category>> GetById(int id)
        {
            var result = new OperationResult<Category>();
            try
            {
                var category = await _unitOfWork.CategoryDAO.GetByIDAsync(id);
                result.Payload = category;
            }
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
            }
            return result;
        }

        public Task<OperationResult<Category>> GetByFilter(Expression<Func<Category, bool>>? predicate)
        {
            var result = new OperationResult<Category>();
            try
            {
                if(predicate == null)
                    throw new Exception("Predicate is null");
                var category = _unitOfWork.CategoryDAO.Get(predicate).FirstOrDefault();
                result.Payload = category;
            }
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
            }
            return Task.FromResult(result);
        }

        public async Task<OperationResult<Category>> Create(CategoryCreate requestModel)
        {
            
            var result = new OperationResult<Category>();
            try
            {
                var category = _mapper.Map<Category>(requestModel);
                await _unitOfWork.CategoryDAO.Insert(category);
                if (await _unitOfWork.SaveAsync() > 0)
                {   
                    result.Payload = category; 
                }
                else
                {
                    result.Payload = null;
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Create Category Failed");
                }
                
            }
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
            }
            return result;
        }

        public async Task<OperationResult<bool>> Delete(int id)
        {
            var result = new OperationResult<bool>();
            try
            {
                var category = await _unitOfWork.CategoryDAO.GetByIDAsync(id);
                if (category == null)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Category not found");
                    return result;
                }
                _unitOfWork.CategoryDAO.Delete(category);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    result.IsError = false;
                    result.Payload = true;
                }
                else
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.UnAuthorize, "Delete Category Failed");
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.AddUnknownError(e.Message);
            }

            return result;
        }

        public async Task<OperationResult<Category>> Update(int id, Category requestModel)
        {
            var result = new OperationResult<Category>();
            try
            {
                var category = await _unitOfWork.CategoryDAO.GetByIDAsync(id);
                if (category == null)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Category not found");
                    return result;
                }
                category.CategoryName = requestModel.CategoryName;
                _unitOfWork.CategoryDAO.Update(category);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    result.Payload = category;
                }
                else
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.UnAuthorize, "Update Category Failed");
                }
            }
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
            }

            return result;
        }

        public Task<OperationResult<Pagination<Category>>> GetAccountPaginationAsync(int pageIndex = 0,
            int pageSize = 10)
        {
            throw new NotImplementedException();
        }
    }
}