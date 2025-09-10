namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product
{
    public class UpdateProductDto
    {
        public string ProductName { get; set; }

        public string Description { get; set; }
      //  public Guid CategoryId { get; set; }
       
        public double AvailableQuantity { get; set; }
      //  public DateTime CreatedAT { get; set; }
        public DateTime UpateddAT { get; set; }

        public double UnitPrice { get; set; }
      //  public double TotalPrice => AvailableQuantity * UnitPrice;
    }




}
