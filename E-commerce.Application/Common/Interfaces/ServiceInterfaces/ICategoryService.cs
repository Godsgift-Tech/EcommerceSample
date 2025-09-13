using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;

namespace E_commerce.Application.Common.Interfaces.ServiceInterfaces
{
    public interface ICategoryService
    {
        Task<ServiceResponse<CreateCategoryDto>> CreateCategoryAsync(CreateCategoryDto categoryDto, string userId);
        Task <ServiceResponse<CategoryDto>> UpdateCategoryAsync(Guid categoryId, UpdateCategoryDto category);
        Task<ServiceResponse<bool>> DeleteCategoryAsync(Guid categoryId);
        Task<ServiceResponse<CreateCategoryDto>>GetCategoryById(Guid categoryId);
        Task<ServiceResponse<PagedResult<CategoryDto>>> GetAllCategoriesAsync(int pageNumber, int pageSize);



    }
}
