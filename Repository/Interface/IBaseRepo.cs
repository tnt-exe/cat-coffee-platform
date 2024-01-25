using DAO.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBaseRepo<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        Task<OperationResult<IEnumerable<TResponse>>> GetAll(Expression<Func<TRequest, bool>>? predicate);
        Task<OperationResult<TResponse>> GetById(int Id);
        Task<OperationResult<TResponse>> GetByPredicate(Expression<Func<TRequest, bool>>? predicate);
        Task<OperationResult<TResponse>> Create(TRequest requestModel);
        Task<OperationResult<bool>> Delete(int id);
        Task<OperationResult<TResponse>> Update(int Id, TRequest requestModel);
        Task<OperationResult<Pagination<TResponse>>> GetAccountPaginationAsync(int pageIndex = 0, int pageSize = 10);
    }
}
