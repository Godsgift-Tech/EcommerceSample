using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Payment
{
    public class PaymentDto
    {
    }

    public class CreatePaymentDto
    {
        public Guid OrderId { get; set; }
        public string PaymentMethod { get; set; } = "Card";
        public double Amount { get; set; }
        public string Currency { get; set; } = "NGN";
    }

    public class UpdatePaymentDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = "Pending";  // Pending, Successful, Failed
        public string? TransactionReference { get; set; }
    }

    public class GetPaymentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionReference { get; set; }

        // Extra info for frontend
        public double OrderAmount { get; set; }
    }
}

