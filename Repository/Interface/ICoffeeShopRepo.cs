using DAO.Helper;
using DTO.CoffeeShopDTO;

namespace Repository.Interface
{
    public interface ICoffeeShopRepo
    {
        Task<OperationResult<IEnumerable<CoffeeShopResponseDTO>>> GetAllCoffeeShops();
        Task<OperationResult<CoffeeShopResponseDTO>> Create(CoffeeShopCreate resource);
        Task<OperationResult<CoffeeShopResponseDTO>> GetByID(int id);
        Task<OperationResult<CoffeeShopResponseDTO>> GetByManagerID(Guid id);
        Task<OperationResult<CoffeeShopResponseDTO>> Update(CoffeeShopUpdate resource, int id);
        Task<OperationResult<object>> Deleted(int id);
    }
}
