using AutoMapper;
using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.Interfaces.UnitOfWork;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Payment;
using E_commerce.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOFWorks _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        // Cache key for storing all payments (general listing)
        private const string AllPaymentsCacheKey = "all_payments_cache";

        public PaymentService(IUnitOFWorks unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ServiceResponse<bool>> DeletePaymentAsync(Guid id)
        {
            try
            {
                var deleted = await _unitOfWork.PaymentRepository.DeletePayment(id);
                if (!deleted)
                    return new ServiceResponse<bool>(false, false, "Payment not found");

                await _unitOfWork.Completed();

                //  Invalidate related cache entries to prevent stale data
                _cache.Remove($"Payment_{id}");       // remove specific payment cache
                _cache.Remove(AllPaymentsCacheKey);   // remove general cache for all payments

                return new ServiceResponse<bool>(true, true, "Payment deleted successfully");
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>(false, false, $"Error deleting payment: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<PagedResult<GetPaymentDto>>> GetPaymentsByUserAsync(string userId, int pageNumber, int pageSize)
        {
            // Avoid avoid data overlap by Using a unique cache key per user and pagination 
            string cacheKey = $"UserPayments_{userId}_Page{pageNumber}_Size{pageSize}";

            // Try fetching data from cache first
            if (!_cache.TryGetValue(cacheKey, out PagedResult<GetPaymentDto>? cachedPayments))
            {
                //  fetch from the repository when not in cache
                var pagedPayments = await _unitOfWork.PaymentRepository.GetPaymentByUserId(userId, pageNumber, pageSize);

                if (!pagedPayments.Items.Any())
                    return new ServiceResponse<PagedResult<GetPaymentDto>>(null!, false, "No payments found");

                // Map entity data to DTOs
                var mappedPayments = _mapper.Map<List<GetPaymentDto>>(pagedPayments.Items);

                // Build the paged result
                cachedPayments = new PagedResult<GetPaymentDto>
                {
                    TotalCount = pagedPayments.TotalCount,
                    PageNumber = pagedPayments.PageNumber,
                    PageSize = pagedPayments.PageSize,
                    Items = mappedPayments
                };

                //  Cache the result for this specific user and page for 2 minutes
                _cache.Set(cacheKey, cachedPayments, TimeSpan.FromMinutes(2));
            }

            // Return  either cached or freshly fetched data
            return new ServiceResponse<PagedResult<GetPaymentDto>>(cachedPayments, true, "Payments retrieved successfully");
        }

        public async Task<ServiceResponse<GetPaymentDto>> MakePaymentAsync(CreatePaymentDto dto, string userId)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = dto.OrderId,
                UserId = userId,
                PaymentMethod = dto.PaymentMethod,
                Amount = dto.Amount,
                Currency = dto.Currency,
                Status = "Pending",
                PaymentDate = DateTime.UtcNow
            };

            //  Repository now returns a hydrated payment (includes Order)
            var savedPayment = await _unitOfWork.PaymentRepository.MakePayment(payment);

            if (savedPayment == null)
                return new ServiceResponse<GetPaymentDto>(null!, false, "Failed to create payment");

            var mapped = _mapper.Map<GetPaymentDto>(savedPayment);

            // ✅ Cache results
            _cache.Set($"Payment_{savedPayment.Id}", mapped, TimeSpan.FromMinutes(5));
            _cache.Remove(AllPaymentsCacheKey);

            return new ServiceResponse<GetPaymentDto>(mapped, true, "Payment created successfully");
        }



        public async Task<ServiceResponse<GetPaymentDto>> UpdatePaymentAsync(UpdatePaymentDto dto)
        {
            // Retrieve the payment from the database
            var payment = await _unitOfWork.PaymentRepository.GetPaymentById(dto.Id);
            if (payment == null)
                return new ServiceResponse<GetPaymentDto>(null!, false, "Payment not found");

            // Update  these fields
            payment.Status = dto.Status;
            payment.TransactionReference = dto.TransactionReference;

            await _unitOfWork.PaymentRepository.UpdatePayment(payment);
            await _unitOfWork.Completed();

            var mapped = _mapper.Map<GetPaymentDto>(payment);

            //  Update cache for this specific payment
            _cache.Set($"Payment_{payment.Id}", mapped, TimeSpan.FromMinutes(5));

            // Remove the liist from cache
            _cache.Remove(AllPaymentsCacheKey);

            return new ServiceResponse<GetPaymentDto>(mapped, true, "Payment updated successfully");
        }
    }
}
