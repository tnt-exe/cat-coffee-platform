﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.ProductDTO
{
    public record ProductCreate
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public int CoffeeShopId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
