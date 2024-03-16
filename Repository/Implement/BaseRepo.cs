using DAO.Helper;
using Repository.Interface;
using System.Linq.Expressions;

namespace Repository.Implement
{
    public abstract class BaseRepo<TEntity> : IBaseRepo<TEntity> where TEntity : class
    {

        public BaseRepo() { }

        public Task<OperationResult<TEntity>> Create(TEntity requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Pagination<TEntity>>> GetAccountPaginationAsync(int pageIndex = 0, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<TEntity>>> GetAll(Expression<Func<TEntity, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TEntity>> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TEntity>> GetByPredicate(Expression<Func<TEntity, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TEntity>> Update(int Id, TEntity requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
