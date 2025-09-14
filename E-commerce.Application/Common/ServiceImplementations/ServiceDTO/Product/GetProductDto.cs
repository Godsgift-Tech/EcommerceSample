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
        //   public Guid OrderId { get; set; }

        //  public ICollection<Order> ProductOrder { get; set; } = new List<Order>();
        public double AvailableQuantity { get; set; }
        //public double QuantityDemanded { get; set; }
        //public double RemainingQuantity => AvailableQuantity - QuantityDemanded;
        public DateTime CreatedAT { get; set; }
        public DateTime UpateddAT { get; set; }

        public double UnitPrice { get; set; }
    }


}
