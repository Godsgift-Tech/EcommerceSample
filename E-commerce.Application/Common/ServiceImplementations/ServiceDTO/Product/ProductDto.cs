using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product
{
    public class ProductDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [MaxLength(30)]

        public string ProductName { get; set; }


        public string Description { get; set; }
        [JsonIgnore]

        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
      
        public double AvailableQuantity { get; set; }
      
        public DateTime CreatedAT { get; set; }
        public DateTime UpdatedAT { get; set; }

        public double UnitPrice { get; set; }
        public string Currency { get; set; } = "NGN";
        public string DisplayPrice => $"{Currency} {UnitPrice:N2} ";

    }


}
