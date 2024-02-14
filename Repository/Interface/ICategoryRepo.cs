using System.Linq.Expressions;
using BusinessObject.Model;
using DAO.Helper;
using DTO.CategoryDTO;

namespace Repository.Interface
{
    public interface ICategoryRepo
    {
        Task<OperationResult<IEnumerable<Category>>> GetAll(Expression<Func<Category, bool>>? filter,
             int? pageIndex = 0, int? pageSize = 10, string[]? includeProperties = null);
        Task<OperationResult<Category>> GetById(int id);
        Task<OperationResult<Category>> GetByFilter(Expression<Func<Category, bool>>? filter);
        Task<OperationResult<Category>> Create(CategoryUpsert requestModel);
        Task<OperationResult<bool>> Delete(int id);
        Task<OperationResult<Category>> Update(int id, CategoryUpsert requestModel);
        Task<OperationResult<Pagination<Category>>> GetAccountPaginationAsync(int pageIndex = 0, int pageSize = 10);
    }
}
