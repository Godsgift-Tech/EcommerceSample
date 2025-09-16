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
    public class CreateProductDto
    {

        [JsonIgnore]
        public Guid Id { get; set; }
        [MaxLength(30)]

        public string ProductName { get; set; }

        public string Description { get; set; }
        public Guid CategoryId { get; set; }
     
        public double AvailableQuantity { get; set; }
     
        public DateTime CreatedAT { get; set; }
        public string Currency { get; set; } = "NGN";
        public double UnitPrice { get; set; }

    }
}
