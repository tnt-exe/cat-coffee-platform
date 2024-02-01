using DTO.CoffeeShopDTO;
using DAO.Helper;
using BusinessObject.Model;

namespace Repository.Interface
{
    public interface ICoffeeShopRepo
    {
        Task<OperationResult<IEnumerable<CoffeeShopResponseDTO>>> GetAllCoffeeShops();
        Task<OperationResult<CoffeeShopCreate>> Create(CoffeeShopCreate resource);
        Task<OperationResult<CoffeeShop>> GetByID(int id);
        Task<OperationResult<CoffeeShopResponseDTO>> Create(CoffeeShopResponseDTO resource, int id);
    }
}
