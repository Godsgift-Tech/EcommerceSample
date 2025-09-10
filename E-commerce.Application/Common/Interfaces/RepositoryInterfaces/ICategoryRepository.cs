using E_commerce.Core.Entities;

namespace E_commerce.Application.Common.Interfaces.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryById(Guid id);
    }
}
