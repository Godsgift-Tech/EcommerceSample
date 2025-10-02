using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Category> ProductCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> ProductOrders { get; set; } = new List<Order>();
        public ICollection<Payment> AllPayments { get; set; } = new List<Payment>();

    }
}
