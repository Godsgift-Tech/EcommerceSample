using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.Interfaces.ServiceInterfaces
{
    public interface IPaymentService
    {
        Task<ServiceResponse<GetPaymentDto>> MakePaymentAsync(CreatePaymentDto dto, string userId);
        Task<ServiceResponse<GetPaymentDto>> UpdatePaymentAsync(UpdatePaymentDto dto);
        Task<ServiceResponse<PagedResult<GetPaymentDto>>> GetPaymentsByUserAsync(string userId, int pageNumber, int pageSize);
        Task<ServiceResponse<bool>> DeletePaymentAsync(Guid id);
    }
}
