using E_commerce.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product
{
    public class GetProductDto
    {
      
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
        public DateTime? UpdatedAT { get; set; } 
        public string Currency { get; set; } = "NGN";
        public double UnitPrice { get; set; }
        public string DisplayPrice => $"{Currency} {UnitPrice:N2} ";
        public string UpdatedAtDisplay
     => UpdatedAT.HasValue ? UpdatedAT.Value.ToString("yyyy-MM-dd HH:mm") : "Not yet updated";


    }


}
