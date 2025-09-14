using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.Interfaces.ServiceInterfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<CreateProductDto>> CreateProductAsync(CreateProductDto product);
        Task<ServiceResponse<ProductDto>> UpdateProductAsync(Guid productId, UpdateProductDto product);
        Task<ServiceResponse<bool>> DeletProductAsync(Guid productId);
        Task<ServiceResponse<CreateProductDto>> GetProductById(Guid productId);
        Task<ServiceResponse<ProductSummaryDto>> GetProductSummaryHistory(Guid productId);
        Task<ServiceResponse<PagedResult<GetProductDto>>> GetAllProductAsync(int pageNumber, int pageSize);

       
    }
}
