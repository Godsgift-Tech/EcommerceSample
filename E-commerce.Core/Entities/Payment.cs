using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Entities
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }   // link payment to a specific order
        public Order Order { get; set; }    // navigation property

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "Card"; // Card, BankTransfer, Wallet, etc.

        [Required]
        public double Amount { get; set; }   // amount paid (should equal Order.OrderAmount ideally)
         // User  relationship
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [MaxLength(20)]
        public string Currency { get; set; } = "NGN";

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending"; // Pending, Successful, Failed

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? TransactionReference { get; set; } // from payment gateway (e.g. Paystack, Stripe)
    }
}
