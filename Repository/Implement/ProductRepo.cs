using System.Linq.Expressions;
using AutoMapper;
using BusinessObject.Model;
using DAO.Helper;
using DAO.UnitOfWork;
using DTO.ProductDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implement
{
    public class ProductRepo : IProductRepo
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductRepo(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<Product>>> GetAll(int shopId,
            Expression<Func<Product, bool>>? filter, int? pageIndex, int? pageSize, string[]? includeProperties = null)
        {
            var result = new OperationResult<IEnumerable<Product>>();
            try
            {
                var products =  _unitOfWork.ProductDAO.Get().AsQueryable();
                if (products.ToList().Count == 0)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Does not have any product");
                }
                else
                {
                    products = products.Where(x => x.CoffeeShopId == shopId);
                    if (filter != null)
                    {
                        products = products.Where(filter);
                    }
                    if (includeProperties != null)
                    {
                        foreach (var includeProperty in includeProperties)
                        {
                            products = products.Include(includeProperty);
                        }
                    }
                    result.Payload = await products.Skip(pageIndex!.Value * pageSize!.Value).Take(pageSize.Value).ToListAsync();
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;
        }

        public Task<OperationResult<Product>> GetById(int shopId, int id)
        {
            var result = new OperationResult<Product>();
            try
            {
                var product = _unitOfWork.ProductDAO.Get().FirstOrDefault(x => x.CoffeeShopId == shopId && x.ProductId == id);
                if (product == null)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Product not found");
                }
                else
                {
                    result.Payload = product;
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.AddError(ErrorCode.ServerError, e.Message);
            }

            return Task.FromResult(result);
        }

        public async Task<OperationResult<Product>> Create(ProductCreate requestModel)
        {
            var result = new OperationResult<Product>();
            try
            {
                var product = _mapper.Map<Product>(requestModel);
                await _unitOfWork.ProductDAO.Insert(product);
                var count = await _unitOfWork.SaveAsync();
                if(count == 0)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.ServerError, "Create product failed");
                }
                else
                {
                    result.Payload = product;
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;
        }

        public Task<OperationResult<bool>> Delete(int shopId, int productId)
        {
            var result = new OperationResult<bool>();
            try
            {
                var product = _unitOfWork.ProductDAO.Get().FirstOrDefault(x => x.CoffeeShopId == shopId && x.ProductId == productId);
                if (product == null)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Product not found");
                }
                else
                {
                    _unitOfWork.ProductDAO.Delete(product);
                    var count = _unitOfWork.SaveAsync().Result;
                    if (count == 0)
                    {
                        result.IsError = true;
                        result.AddError(ErrorCode.ServerError, "Delete product failed");
                    }
                    else
                    {
                        result.Payload = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return Task.FromResult(result);
        }

        public Task<OperationResult<Product>> Update(int shopId, int productId, ProductUpdate requestModel)
        {
            var result = new OperationResult<Product>();
            try
            {
                var product = _unitOfWork.ProductDAO.Get().FirstOrDefault(x => x.CoffeeShopId == shopId && x.ProductId == productId);
                if (product == null)
                {
                    result.IsError = true;
                    result.AddError(ErrorCode.NotFound, "Product not found");
                }
                else
                {
                    _mapper.Map(requestModel, product);
                    _unitOfWork.ProductDAO.Update(product);
                    var count = _unitOfWork.SaveAsync().Result;
                    if (count == 0)
                    {
                        result.IsError = true;
                        result.AddError(ErrorCode.ServerError, "Update product failed");
                    }
                    else
                    {
                        result.Payload = product;
                    }
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return Task.FromResult(result);
        }
    }
}