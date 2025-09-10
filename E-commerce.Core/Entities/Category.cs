using System.ComponentModel.DataAnnotations;

namespace E_commerce.Core.Entities
{
    public class Category
    {
        [Key]

        public Guid  Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription{ get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime CreatedAT { get; set; }
        public DateTime UpateddAT { get; set; }
        public ICollection<Product> Products { get; set; }= new List <Product>();

    }
}

