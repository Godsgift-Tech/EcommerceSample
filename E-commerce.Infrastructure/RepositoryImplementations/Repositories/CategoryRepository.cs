using AutoMapper;
using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Core.Entities;
using E_commerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Infrastructure.RepositoryImplementations.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EcomDbContext _db;
        public CategoryRepository(EcomDbContext db)
        {
            _db = db;
        }
        public async Task CreateCategoryAsync(Category category) => await _db.Categories.AddAsync(category);

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var getCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (getCategory != null) _db.Remove(getCategory);
            return true;

        }


        public async Task<PagedResult<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
           
            var query = _db.Categories
                          .AsNoTracking()
                          .OrderBy(p => p.Id); // Sort deterministically

            return await PagedResult<Category>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Category?> GetCategoryById(Guid id) => await _db.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Category?> GetCategoryByName(string categoryName)
        {
           return await _db.Categories.AsNoTracking()
          .FirstOrDefaultAsync(c => c.CategoryName==categoryName.ToLower());
                
        }

        //public async Task UpdateCategoryAsync(Category category)
        //{
        //    var update = await _db.Categories.FindAsync(category.Id);
        //       // FirstOrDefaultAsync(c => c.Id == category.Id);
        //    if (update != null)
        //        _db.Update(category);


        //}
        public async Task ReloadAsync(Category category)
        {
            await _db.Entry(category).ReloadAsync();
        }


        public async Task UpdateCategoryAsync(Category category)
        {
            var update = await _db.Categories.FindAsync(category.Id);
            if (update != null)
            {
                // Copy values from detached category to the tracked entity
                _db.Entry(update).CurrentValues.SetValues(category);
            }
        }

    }
}
