using DAO.Helper;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IBaseRepo<TEntity> where TEntity : class
    {
        Task<OperationResult<IEnumerable<TEntity>>> GetAll(Expression<Func<TEntity, bool>>? predicate);
        Task<OperationResult<TEntity>> GetById(int id);
        Task<OperationResult<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>>? predicate);
        Task<OperationResult<TEntity>> Create(TEntity requestModel);
        Task<OperationResult<bool>> Delete(int id);
        Task<OperationResult<TEntity>> Update(int id, TEntity requestModel);
        Task<OperationResult<Pagination<TEntity>>> GetAccountPaginationAsync(int pageIndex = 0, int pageSize = 10);
    }
}
