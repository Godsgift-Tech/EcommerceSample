using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Core.Entities;
using E_commerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.RepositoryImplementations.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcomDbContext _db;

        public OrderRepository(EcomDbContext db)
        {
            _db = db;
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (order == null) return false;

            _db.Orders.Remove(order);
            return true;
        }

       
        public async Task<PagedResult<Order>> GetAll(string? userId, int pageNumber, int pageSize)
        {
            IQueryable<Order> query = _db.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(userId))
                query = query.Where(o => o.UserId == userId);

            query = query.OrderBy(o => o.Id); // apply ordering once at the end

            return await PagedResult<Order>.CreateAsync(query, pageNumber, pageSize);
        }


        public async Task<Order?> GetOrder(Guid id) =>
            await _db.Orders
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);




        public async Task<Order?> MakeOrderAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
            return order; // just returns the tracked entity, not saved yet
        }

        public IQueryable<Order> QueryByUserId(string userId)
        {
            return _db.Orders.Include(o=> o.Items)
                .Where(o=>o.UserId ==userId).AsQueryable();
        }
      
        public async Task Update(Order order)
        {
            // load existing order with items
            var existingOrder = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (existingOrder != null)
            {
                // update scalars
                _db.Entry(existingOrder).CurrentValues.SetValues(order);

                // sync items (simple approach: replace all)
                existingOrder.Items.Clear();
                foreach (var item in order.Items)
                {
                    existingOrder.Items.Add(item);
                }
            }
        }

     

    }
}
