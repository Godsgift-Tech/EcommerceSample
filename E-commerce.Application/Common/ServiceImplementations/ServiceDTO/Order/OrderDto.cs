using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Order
{
    public class OrderDto
    {
        
        public Guid Id { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        public double OrderAmount { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? OrderUpdatedAT { get; set; }
        

        // Represents items in the order (with product details)
        public List<OrderProductDto> Products { get; set; } = new();
    }

    public class GetOrderDto
    {

        public double OrderAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Paid, Cancelled, etc.

        public DateTime OrderTime { get; set; }
        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto> { };

    }
    public class OrderProductDto
    {
        [JsonIgnore]

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string Currency { get; set; } = "NGN";
        public double TotalPrice { get; set; }
        public string Amount => $"{Currency} {TotalPrice:N2} ";
        //  DisplayPrice
        //public string DisplayPrice => $"{Currency} {UnitPrice:N2} ";


    }

    public class CreateOrderDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public List<CreateOrderItemDto> Products { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public double Quantity { get; set; }
    }

    public class UpdateOrderDto
    {
        [Required]
        public List<CreateOrderItemDto> Products { get; set; } = new();
    }
}
