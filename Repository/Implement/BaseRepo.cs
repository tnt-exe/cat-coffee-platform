using DAO.Helper;
using Repository.Interface;
using System.Linq.Expressions;

namespace Repository.Implement
{
    public abstract class BaseRepo<TRequest, TResponse> : IBaseRepo<TRequest, TResponse> where TRequest : class where TResponse : class
    {

        public BaseRepo() { }

        public Task<OperationResult<TResponse>> Create(TRequest requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Pagination<TResponse>>> GetAccountPaginationAsync(int pageIndex = 0, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<TResponse>>> GetAll(Expression<Func<TRequest, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TResponse>> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TResponse>> GetByPredicate(Expression<Func<TRequest, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<TResponse>> Update(int Id, TRequest requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
