using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessObject.Model;
using CatCoffeePlatformAPI.Controllers.Base;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Implement;
using Repository.Interface;

namespace CatCoffeePlatformAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] [Required] int shopId,
            [FromQuery] int? productId,
            [FromQuery] string? productName,
            [FromQuery] int? categoryId,
            [FromQuery] int? pageIndex = 0,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string[]? includeProperties = null)
        {
            var response = await _productRepo.GetAll(
                shopId,
                x =>
                    (x.CoffeeShopId == shopId) ||
                    (productId == null || x.ProductId == productId) ||
                    (productName == null || x.ProductName.Contains(productName)) ||
                    (categoryId == null || x.CategoryId == categoryId), pageIndex, pageSize, includeProperties);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetAllByFilter(
            [FromQuery] [Required] int shopId,
            [FromQuery] int? productId,
            [FromQuery] string? productName,
            [FromQuery] int? categoryId,
            [FromQuery] int? pageIndex = 0,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string[]? includeProperties = null)
        {
            // Build the predicate dynamically based on the provided parameters
            Expression<Func<Product, bool>> predicate = x => x.CoffeeShopId == shopId;

            if (productId != null || productName != null || categoryId != null)
            {
                if (productId != null)
                {
                    predicate = CombinePredicatesWithAnd(predicate, x => x.ProductId == productId);
                }

                if (productName != null)
                {
                    predicate = CombinePredicatesWithAnd(predicate, x => x.ProductName.Contains(productName));
                }

                if (categoryId != null)
                {
                    predicate = CombinePredicatesWithAnd(predicate, x => x.CategoryId == categoryId);
                }
            }
            else
            {
                predicate = x => x.CoffeeShopId == shopId;
            }

            var response = await _productRepo.GetAll(shopId, predicate, pageIndex, pageSize, includeProperties);

            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreate product)
        {
            var response = await _productRepo.Create(product);
            return response.IsError
                ? HandleErrorResponse(response.Errors)
                : Created("/api/product", response.Payload);
        }

        [HttpGet("shopId={shopId}&productId={productId}")]
        public async Task<IActionResult> GetById(
            [Required] int shopId,
            [Required] int productId)
        {
            var response = await _productRepo.GetById(shopId, productId);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromQuery] [Required] int shopId,
            [FromQuery] [Required] int productId,
            [FromBody] ProductUpdate product)
        {
            if (String.IsNullOrEmpty(product.ProductName) && product.Price == 0 && product.Quantity == 0 &&
                String.IsNullOrEmpty(product.Unit) && product.CategoryId == 0)
            {
                return BadRequest("Please provide at least one field to update.");
            }

            var response = await _productRepo.Update(shopId, productId, product);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int shopId, int productId)
        {
            var response = await _productRepo.Delete(shopId, productId);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
        }

        // Helper method to combine predicates using AND
        private Expression<Func<T, bool>> CombinePredicatesWithAnd<T>(
            Expression<Func<T, bool>> predicate1,
            Expression<Func<T, bool>> predicate2)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = Expression.AndAlso(
                Expression.Invoke(predicate1, param),
                Expression.Invoke(predicate2, param));

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}