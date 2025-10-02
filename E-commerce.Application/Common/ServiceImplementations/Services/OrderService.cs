using AutoMapper;
using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.Interfaces.UnitOfWork;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Order;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product;
using E_commerce.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOFWorks _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private const string AllOrderCacheKey = "all_order_cacheKey";

        public OrderService(IUnitOFWorks unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<ServiceResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            // creates a new Order in memory 
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId.ToString(),  // ties userId to the user
                OrderTime = DateTime.UtcNow,
                Items = new List<OrderItem>()   // empty string to hold orderItems
            };

            double totalAmount = 0;

            foreach (var item in dto.Products)
            {
                var product = await _unitOfWork.ProductRepository.GetProductById(item.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product {item.ProductId} not found");

                if (item.Quantity > product.AvailableQuantity)
                    throw new InvalidOperationException($"Not enough stock for {product.ProductName}");

                //  Deduct from stock
                product.AvailableQuantity -= item.Quantity;

                // Optional: track total demand for analytics/reporting
                product.QuantityDemanded += item.Quantity;

                await _unitOfWork.ProductRepository.UpdateProductAsync(product);

                // Create order item
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice
                };

                order.Items.Add(orderItem);

                totalAmount += product.UnitPrice * item.Quantity;
            }

            order.OrderAmount = totalAmount;

            // Add order but don’t save yet
            await _unitOfWork.OrderRepository.MakeOrderAsync(order);

            // saves changes (order + product stock updates)
            await _unitOfWork.Completed();

            // Reload the order with items + product details
            var savedOrder = await _unitOfWork.OrderRepository.GetOrder(order.Id);

            var createdOrder = _mapper.Map<OrderDto>(savedOrder);

            // Cache the created order
            _memoryCache.Set($"{order.Id}", createdOrder, TimeSpan.FromMinutes(5));
            _memoryCache.Remove(AllOrderCacheKey);

            return new ServiceResponse<OrderDto>(createdOrder, true, "Order has been placed successfully");
        }

        public async Task <ServiceResponse<bool>> DeleteOrderAsync(Guid orderId)
        {
            try
            {
                var result = await _unitOfWork.OrderRepository.DeleteOrder(orderId);
              if  (!result) return new ServiceResponse<bool>(false, false, "This order was not found or has been cancelled already");
                await _unitOfWork.Completed();
                _memoryCache.Remove($"OrderId{orderId}");
                _memoryCache.Remove(AllOrderCacheKey);
                return new ServiceResponse<bool>(result, true, "Order was successfully deleted");


            }
            catch (Exception ex)
            {

                return new ServiceResponse<bool>(false, false, $"Error encountered while deleting order: {ex.Message} ");

            }
        }

   

        public async Task<ServiceResponse<PagedResult<GetOrderDto>>> GetAllOrdersAsync(string? userId, int pageNumber, int pageSize)
        {
            var pagedOrder = await _unitOfWork.OrderRepository.GetAll(userId, pageNumber, pageSize);

                if (pagedOrder.Items.Count == 0)
                return new ServiceResponse<PagedResult<GetOrderDto>>(null!, false, $"There is no order found for UserID : {userId}");

            var orders = _mapper.Map<List<GetOrderDto>>(pagedOrder.Items);

            var result = new PagedResult<GetOrderDto>
            {
                TotalCount = pagedOrder.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = orders
            };

            return new ServiceResponse<PagedResult<GetOrderDto>>(result, true, "Orders retrieved successfully");
        }

        public async Task <ServiceResponse<GetOrderDto>> GetOrderByIdAsync(Guid orderId)
        {
            var existingOrder = await _unitOfWork.OrderRepository.GetOrder(orderId);
            if (existingOrder == null) return new ServiceResponse<GetOrderDto>(null!, false, "Order was not found");
            var orderInfo = _mapper.Map<GetOrderDto>(existingOrder);
            return new ServiceResponse<GetOrderDto>(orderInfo, true, "Your order was retrieved successfully");

        }



        public async Task <ServiceResponse<OrderDto>> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto)
        {
            //  Load existing order
            var existingOrder = await _unitOfWork.OrderRepository.GetOrder(orderId);
            //      _orderRepository.GetByIdAsync(dto.Id);
            if (existingOrder == null)
                // throw new KeyNotFoundException("Order not found");

                return new ServiceResponse<OrderDto>(null!, false, "Order was not found or has been removed alreaady");
            //  Map scalars (except Items & UserId)
            _mapper.Map(dto, existingOrder);

            //  Build fresh list of OrderItems
            var newItems = new List<OrderItem>();
            foreach (var itemDto in dto.Products)
            {
                var product = await _unitOfWork.ProductRepository.GetProductById(itemDto.ProductId);
                //   _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    //throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found");
                    return new ServiceResponse<OrderDto>(null!, false, $"Product with ID {itemDto.ProductId} not found");

                //  check stock before update
                if (product.RemainingQuantity < itemDto.Quantity)
                    //   throw new InvalidOperationException($"Not enough stock for {product.ProductName}");
                    return new ServiceResponse<OrderDto>(null!, false, $"Not enough stock for {product.ProductName}");
                
                newItems.Add(new OrderItem
                {
                      Product = product,
                    
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.UnitPrice
                });
            }

            // Replace Items (repo handles remove & add)
            existingOrder.Items = newItems;


            // Recalculate OrderAmount
            existingOrder.OrderAmount = newItems.Sum(i => i.Quantity * i.UnitPrice);

            //  Save changes
            await _unitOfWork.OrderRepository.Update(existingOrder);
            await _unitOfWork.Completed();
            // reload with product details
            var updated = await _unitOfWork.OrderRepository.GetOrder(orderId);


            //  Return mapped DTO
            var updateOrder =  _mapper.Map<OrderDto>(existingOrder);
            updateOrder.OrderUpdatedAT = DateTime.Now;
            // remove cache
            _memoryCache.Remove($"Order_{orderId}");
            _memoryCache.Remove(AllOrderCacheKey);
            return new ServiceResponse<OrderDto>(updateOrder, true, "Your order was updated successfully");
        }

      
      

    }
}
