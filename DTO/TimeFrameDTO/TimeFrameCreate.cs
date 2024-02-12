﻿using System.ComponentModel.DataAnnotations;

namespace DTO.TimeFrameDTO
{
    public record TimeFrameCreate
    {
        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "Invalid Price")]
        public decimal Price { get; set; }

        public int CoffeeShopId { get; set; }
    }
}
