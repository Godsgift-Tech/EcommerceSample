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
     //   public Category Category { get; set; }
      //  public Guid OrderId { get; set; }

     //   public ICollection<Order> ProductOrder { get; set; } = new List<Order>();
        public double AvailableQuantity { get; set; }
      //  public double QuantityDemanded { get; set; }
      //  public double RemainingQuantity => AvailableQuantity - QuantityDemanded;
        public DateTime CreatedAT { get; set; }
    //    public DateTime UpateddAT { get; set; }

        public double UnitPrice { get; set; }
    //    public double TotalPrice => AvailableQuantity * UnitPrice;

    }
}
