using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(30)]

        public string ProductName { get; set; }

        public string Description { get; set; }
        // Category relationship
        public Guid CategoryId { get; set; }
        public Category Category{ get; set; }

        // Link to OrderItems for many-to-many relationship
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public double AvailableQuantity { get; set; }
        public double QuantityDemanded { get; set; }
        public double RemainingQuantity => AvailableQuantity - QuantityDemanded;
        public DateTime CreatedAT { get; set; } 
        public DateTime? UpdatedAT { get; set; } 
       // User  relationship
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public double UnitPrice { get; set; }
        [MaxLength(3)]
        public string Currency { get; set; } = "NGN";
        public double TotalPrice => AvailableQuantity * UnitPrice;


        // Display values with currency
        public string DisplayPrice => $"{Currency} {UnitPrice:N2} ";

        public string Amount => $"{Currency} {TotalPrice:C}";



    }
}

