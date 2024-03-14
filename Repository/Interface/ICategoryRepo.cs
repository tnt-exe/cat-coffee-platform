using DAO.Helper;
using DTO;

namespace Repository.Interface
{
    public interface ICategoryRepo
    {
        Task<OperationResult<IEnumerable<CategoryDto>>> GetCategories();
        Task<OperationResult<CategoryDto>> GetCategoryById(int id);
        Task<OperationResult<CategoryCreate>> CreateCategory(CategoryCreate categoryCreate);
        Task<OperationResult<CategoryUpdate>> UpdateCategory(CategoryUpdate categoryUpdate);
    }
}
