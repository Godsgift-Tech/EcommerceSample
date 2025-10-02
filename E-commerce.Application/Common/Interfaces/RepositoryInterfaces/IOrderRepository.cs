using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.Interfaces.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task <Order?> MakeOrderAsync(Order order);

        Task Update(Order order);
        Task <bool> DeleteOrder(Guid id);
        Task<Order?> GetOrder(Guid id);
       // Task <List<Order>> GetOrderBy(string userId);
        IQueryable<Order> QueryByUserId(string userId);

        Task<PagedResult<Order>> GetAll(string? userId, int pageNumber, int pageSize);
      
    }
}
