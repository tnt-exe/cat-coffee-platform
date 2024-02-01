using DAO.Helper;
using DTO.CoffeeShopDTO;
using Repository.Interface;
using DAO.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Model;

namespace Repository.Implement
{
    public class CoffeeShopRepo : ICoffeeShopRepo
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CoffeeShopRepo(IMapper mapper)
        {
            _unitOfWork = new UnitOfWork();
            _mapper = mapper;
        }
        public async Task<OperationResult<CoffeeShopCreate>> Create(CoffeeShopCreate resource)
        {
            var result = new OperationResult<CoffeeShopCreate>
            {
                IsError = false,
            };
            try
            {
                var newCoffeeShop = _mapper.Map<CoffeeShop>(resource);

                await _unitOfWork.CoffeeShopDAO.Insert(newCoffeeShop);
                await _unitOfWork.SaveAsync();
                result.Payload = _mapper.Map<CoffeeShopCreate>(newCoffeeShop);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public Task<OperationResult<CoffeeShopResponseDTO>> Create(CoffeeShopResponseDTO resource, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<IEnumerable<CoffeeShopResponseDTO>>> GetAllCoffeeShops()
        {
            var shopList = await _unitOfWork.CoffeeShopDAO.Get().ToListAsync();
            var response = _mapper.Map<IEnumerable<CoffeeShopResponseDTO>>(shopList);
            var result = new OperationResult<IEnumerable<CoffeeShopResponseDTO>>()
            {
                Payload = response,
                IsError = false
            };

            return result;
        }

        public async Task<OperationResult<CoffeeShop>> GetByID(int id)
        {
            var result = new OperationResult<CoffeeShop>
            {
                IsError = false,
            };
            //string[] includeProperties = { nameof(CoffeeShop.Areas), nameof(CoffeeShop.Products), nameof(CoffeeShop.Cats), nameof(CoffeeShop.TimeFrames) };
            var shop = await _unitOfWork.CoffeeShopDAO.GetByIDAsync(id);
            if (shop == null)
            {
                result.AddError(ErrorCode.NotFound, "Not Found Shop");
            }
            result.Payload = shop;

            return result;
        }
    }
}
