using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.Interfaces.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto);
        Task<ServiceResponse<GetOrderDto>> GetOrderByIdAsync(Guid orderId);
        Task<ServiceResponse<PagedResult<GetOrderDto>>> GetAllOrdersAsync(string? userId, int pageNumber, int pageSize);
        Task <ServiceResponse<OrderDto>> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto);
        Task <ServiceResponse<bool>> DeleteOrderAsync(Guid orderId);
    }
}
