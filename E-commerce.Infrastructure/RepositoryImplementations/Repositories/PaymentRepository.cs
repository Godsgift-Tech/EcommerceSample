using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.RepositoryImplementations.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task MakePayment(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
