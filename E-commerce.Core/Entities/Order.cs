using System.ComponentModel.DataAnnotations;

namespace E_commerce.Core.Entities
{
    public class Order
    {
        [Key]

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public double OrderAmount {  get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Paid, Cancelled, etc.

        public DateTime  OrderTime { get; set; }
        public DateTime? OrderUpdatedAT { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }

    
   

  
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }
        public double UnitPrice { get; set; } // captured at time of order
    }




}

