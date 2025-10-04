using E_commerce.Application.Common.ServiceImplementations.Pagination;
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
       // Task MakePayment(Payment payment);
        Task<Payment?> MakePayment(Payment payment);

        Task<Payment?> GetPaymentById(Guid id);
        Task<PagedResult<Payment>> GetPaymentByUserId(string userId, int pageNumber, int pageSize);

        Task UpdatePayment(Payment payment);
        Task<bool> DeletePayment(Guid id);

    }
}
