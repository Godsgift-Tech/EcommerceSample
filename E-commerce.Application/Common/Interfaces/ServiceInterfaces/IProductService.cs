using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product;

namespace E_commerce.Application.Common.Interfaces.ServiceInterfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<CreateProductDto>> CreateProductAsync(CreateProductDto product, string userId);
        Task<ServiceResponse<ProductDto>> UpdateProductAsync(Guid productId, UpdateProductDto product);
        Task<ServiceResponse<bool>> DeletProductAsync(Guid productId);
        Task<ServiceResponse<CreateProductDto>> GetProductById(Guid productId);
        Task<ServiceResponse<CreateProductDto>> GetProductByName(string productName);
        Task<ServiceResponse<ProductSummaryDto>> GetProductSummaryHistory(Guid productId);
        Task<ServiceResponse<PagedResult<GetProductDto>>> GetAllProductAsync(string? categoryName, string categoryId, double? Price, int pageNumber, int pageSize);
       


    }
}
