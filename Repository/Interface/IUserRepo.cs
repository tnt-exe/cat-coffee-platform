using DAO.Helper;
using DTO.UserDTO;

namespace Repository.Interface
{
    public interface IUserRepo
    {
        Task<OperationResult<UserResponseDTO>> Login(UserLogin resource);
        Task<OperationResult<UserResponseDTO>> Register(UserDTO resource);
        Task<OperationResult<UserResponseDTO>> Update(UserDTO resource, Guid id);
        Task<OperationResult<object>> Delete(Guid id);
        Task<OperationResult<UserResponseDTO>> GetById(Guid id);
        Task<OperationResult<IEnumerable<UserResponseDTO>>> Get(int startPage, int endPage, int? quantity, string? name, byte? role, byte? status, int? coffeeShopId, int? managedShopId);
    }
}
