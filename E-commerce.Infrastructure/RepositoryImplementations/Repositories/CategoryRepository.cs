using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Core.Entities;
using E_commerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            var getCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (getCategory != null) _db.Remove(category);
            return true;

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() => await _db.Categories.ToListAsync();

        public async Task<Category?> GetCategoryById(Guid id) => await _db.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task UpdateCategoryAsync(Category category)
        {
            var update = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (update != null)
                _db.Update(category);

        }
    }
}
