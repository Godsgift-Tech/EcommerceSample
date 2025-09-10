using System.ComponentModel.DataAnnotations;

namespace E_commerce.Core.Entities
{
    public class Order
    {
        [Key]

        public Guid Id { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public double OrderAmount {  get; set; }
        public DateTime  OrderTime { get; set; }
    }
}

