using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Core.Entities;
using System.Runtime.InteropServices;

namespace E_commerce.Application.Common.Interfaces.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        // for reloading changes
        Task ReloadAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<PagedResult<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize);
        Task<Category?> GetCategoryById(Guid id);
        Task<Category?> GetCategoryByName(string categoryName);

    }
}
