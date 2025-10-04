using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Core.Entities;
using E_commerce.Infrastructure.Database;
using E_commerce.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.RepositoryImplementations.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EcomDbContext _db;
        public PaymentRepository(EcomDbContext db)
        {
            _db = db;
        }
        public async Task<bool> DeletePayment(Guid id)
        {
            var getpayment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (getpayment != null) _db.Payments.Remove(getpayment);
            return true;
        }

        public async Task<Payment?> GetPaymentById(Guid id)
        {
            return  await _db.Payments.
                Include(p => p.Order).FirstOrDefaultAsync(p => p.Id == id); 
        }


        public async Task<PagedResult<Payment>> GetPaymentByUserId(string userId, int pageNumber, int pageSize)
        {
            // validate paging args
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            //  building query without applying a filter yet
            IQueryable<Payment> query = _db.Payments
                .AsNoTracking()                      // read-only 
                .Include(p => p.Order);

            // apply user filter when userId is provided
            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(p => p.UserId == userId);
            }

            // most recent first 
            query = query.OrderByDescending(p => p.PaymentDate);

            return await PagedResult<Payment>.CreateAsync(query, pageNumber, pageSize);
        }

        //public async Task MakePayment(Payment payment) => await _db.Payments.AddAsync(payment);


        //  // 2️⃣ Reload with related Order
        //    return await _db.Payments
        //        .Include(p => p.Order)
        //        .FirstOrDefaultAsync(p => p.Id == payment.Id);

        public async Task<Payment?> MakePayment(Payment payment)
        {
            await _db.Payments.AddAsync(payment);

            return await _db.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == payment.Id);
        }


        public async Task UpdatePayment(Payment payment)
        {
            var update =  await  _db.Payments.FindAsync(payment.Id);
            if (update != null)
            {
                // Copy values from detached product to the tracked entity
                _db.Entry(update).CurrentValues.SetValues(payment);
            }   
        }
    }
}
