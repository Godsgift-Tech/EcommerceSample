namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product
{
    public class UpdateProductDto
    {
        public string ProductName { get; set; }

        public string Description { get; set; }
      //  public Guid CategoryId { get; set; }
       
        public double AvailableQuantity { get; set; }
      //  public DateTime CreatedAT { get; set; }
        public DateTime UpdatedAT { get; set; }
        public string Currency { get; set; } = "NGN";
        public string DisplayPrice => $"{UnitPrice:N2} {Currency}";


        public double UnitPrice { get; set; }

        //  public double TotalPrice => AvailableQuantity * UnitPrice;
    }




}
