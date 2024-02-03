using System.Linq.Expressions;
using BusinessObject.Model;
using DAO.Helper;
using DTO.ProductDTO;

namespace Repository.Interface
{
    public interface IProductRepo
    {
        Task<OperationResult<IEnumerable<Product>>> GetAll(
            int shopId, 
            Expression<Func<Product, bool>>? filter,
            int? pageIndex = 0, 
            int? pageSize = 10, 
            string[]? includeProperties = null);
        Task<OperationResult<Product>> GetById(int shopId, int id);
        Task<OperationResult<Product>> Create(ProductCreate requestModel);
        Task<OperationResult<bool>> Delete(int shopId, int productId);
        Task<OperationResult<Product>> Update(int shopId,int productId, ProductUpdate requestModel);
    }
}
