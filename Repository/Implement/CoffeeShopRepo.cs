using AutoMapper;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.CoffeeShopDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

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
        public async Task<OperationResult<CoffeeShopResponseDTO>> Create(CoffeeShopCreate resource)
        {
            var result = new OperationResult<CoffeeShopResponseDTO>
            {
                IsError = false,
            };
            try
            {
                var existedEmail = await _unitOfWork.CoffeeShopDAO.Get(filter: s => s.Email == resource.Email).FirstOrDefaultAsync();
                if (existedEmail is not null)
                {
                    result.AddError(ErrorCode.BadRequest, "Email Aldready Existed");
                    return result;
                }

                var newShop = new CoffeeShop()
                {
                    ShopName = resource.ShopName,
                    Address = resource.Address,
                    OpeningTime = TimeOnly.Parse(resource.OpeningTime ?? ""),
                    ClosingTime = TimeOnly.Parse(resource.ClosingTime ?? ""),
                    ContactNumber = resource.ContactNumber,
                    Email = resource.Email,
                    Description = resource.Description,
                    ManagerId = resource.ManagerId,
                    Deleted = false
                };
                await _unitOfWork.CoffeeShopDAO.Insert(newShop);
                await _unitOfWork.SaveAsync();
                var existedUser = await _unitOfWork.UserDAO.GetByIDAsync(resource.ManagerId);
                var existedShop = await _unitOfWork.CoffeeShopDAO.Get(s => s.ManagerId.Equals(resource.ManagerId)).SingleOrDefaultAsync();
                existedUser.ManagerShopId = existedShop!.CoffeeShopId;
                _unitOfWork.UserDAO.Update(existedUser);
                await _unitOfWork.SaveAsync();
                result.Payload = _mapper.Map<CoffeeShopResponseDTO>(newShop);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<CoffeeShopResponseDTO>> Update(CoffeeShopUpdate resource, int id)
        {
            var result = new OperationResult<CoffeeShopResponseDTO>
            {
                IsError = false,
            };
            try
            {
                var existedShop = await _unitOfWork.CoffeeShopDAO.Get(s => s.CoffeeShopId == id && !s.Deleted).FirstOrDefaultAsync();
                if (existedShop is null)
                {
                    result.AddError(ErrorCode.BadRequest, "Shop not found");
                    return result;
                }
                existedShop.ShopName = resource.ShopName;
                existedShop.Address = resource.Address;
                existedShop.OpeningTime = TimeOnly.Parse(resource.OpeningTime ?? "");
                existedShop.ClosingTime = TimeOnly.Parse(resource.ClosingTime ?? "");
                existedShop.ContactNumber = resource.ContactNumber;
                existedShop.Email = resource.Email;
                existedShop.Description = resource.Description;
                existedShop.ManagerId = resource.ManagerId;

                _unitOfWork.CoffeeShopDAO.Update(existedShop);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<CoffeeShopResponseDTO>>> GetAllCoffeeShops()
        {
            string[] includeProperties = { nameof(CoffeeShop.Manager) };
            var shopList = await _unitOfWork.CoffeeShopDAO.Get(filter: s => !s.Deleted, includeProperties: includeProperties).ToListAsync();
            var response = _mapper.Map<IEnumerable<CoffeeShopResponseDTO>>(shopList);
            var result = new OperationResult<IEnumerable<CoffeeShopResponseDTO>>()
            {
                Payload = response,
                IsError = false
            };

            return result;
        }

        public async Task<OperationResult<CoffeeShopResponseDTO>> GetByID(int id)
        {
            var result = new OperationResult<CoffeeShopResponseDTO>
            {
                IsError = false,
            };
            string[] includeProperties = { nameof(CoffeeShop.Manager) };
            var shopEnity = await _unitOfWork.CoffeeShopDAO.Get(filter: s => s.CoffeeShopId == id, includeProperties: includeProperties).SingleOrDefaultAsync();
            var shop = _mapper.Map<CoffeeShopResponseDTO>(shopEnity);
            if (shop == null)
            {
                result.AddError(ErrorCode.NotFound, "Shop not found");
            }
            result.Payload = shop;

            return result;
        }

        public async Task<OperationResult<object>> Deleted(int id)
        {
            var result = new OperationResult<object>
            {
                IsError = false,
            };

            try
            {
                var existedShop = await _unitOfWork.CoffeeShopDAO.Get(s => s.CoffeeShopId == id && !s.Deleted).FirstOrDefaultAsync();
                if (existedShop is null)
                {
                    result.AddError(ErrorCode.BadRequest, "Shop not found");
                    return result;
                }

                existedShop.Deleted = true;
                _unitOfWork.CoffeeShopDAO.Update(existedShop);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }

        public async Task<OperationResult<CoffeeShopResponseDTO>> GetByManagerID(Guid id)
        {
            var result = new OperationResult<CoffeeShopResponseDTO>
            {
                IsError = false,
            };
            string[] includeProperties = { nameof(CoffeeShop.Manager) };
            var shopEnity = await _unitOfWork.CoffeeShopDAO.Get(filter: s => s.ManagerId.Equals(id), includeProperties: includeProperties).SingleOrDefaultAsync();
            var shop = _mapper.Map<CoffeeShopResponseDTO>(shopEnity);
            if (shop == null)
            {
                result.AddError(ErrorCode.NotFound, "Shop not found");
            }
            result.Payload = shop;

            return result;
        }
    }
}
