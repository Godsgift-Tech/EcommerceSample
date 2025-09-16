using E_commerce.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product
{
    public class ProductSummaryDto
    {

        [JsonIgnore]
        public Guid Id { get; set; }
        [MaxLength(30)]

        public string ProductName { get; set; }


        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
        [JsonIgnore]

        public double AvailableQuantity { get; set; }
        [JsonIgnore]
        public double QuantityDemanded { get; set; }
        public double RemainingQuantity => AvailableQuantity - QuantityDemanded;
        public DateTime CreatedAT { get; set; }
        [JsonIgnore]

        public DateTime? UpdatedAT { get; set; }

        public double UnitPrice { get; set; }
        public string Currency { get; set; } = "NGN";
        public string DisplayPrice => $"{UnitPrice:N2} {Currency}";


        // public double TotalPrice => AvailableQuantity * UnitPrice;
    }



}
