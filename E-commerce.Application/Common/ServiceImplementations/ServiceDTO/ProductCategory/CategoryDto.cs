using System.Text.Json.Serialization;

namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory
{
    public class CategoryDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public DateTime CreatedAT { get; set; }
        public DateTime UpateddAT { get; set; }
    }
}
