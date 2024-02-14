﻿using DAO.Helper;
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
                existedShop.Deleted = resource.Deleted;

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

        public async Task<OperationResult<CoffeeShop>> GetByID(int id)
        {
            var result = new OperationResult<CoffeeShop>
            {
                IsError = false,
            };
            var shop = await _unitOfWork.CoffeeShopDAO.GetByIDAsync(id);
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

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
