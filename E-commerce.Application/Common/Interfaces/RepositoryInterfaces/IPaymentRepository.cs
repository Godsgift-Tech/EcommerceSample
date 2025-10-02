using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.Interfaces.RepositoryInterfaces
{
    public interface IPaymentRepository
    {
        Task MakePayment(Payment payment);

        //Task GetPaymentReceipt(Payment payment);
        //Task CheckPaymentDetails(Order order);
    }
}
