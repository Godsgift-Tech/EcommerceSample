using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Core.Entities;
using E_commerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.RepositoryImplementations.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcomDbContext _db;

        public ProductRepository(EcomDbContext db)
        {
            _db = db;
        }

        public async Task CreateProductAsync(Product product) => await _db.Products.AddAsync(product);
        public async Task<bool> DeleteProductAsync(Guid id)
        {

            var getProduct = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (getProduct != null) _db.Products.Remove(getProduct);
            return true;

        }


        public async Task<Product?> GetProductById(Guid id) => await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product?> GetProductByName(string productName)
        {
            return await _db.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductName == productName.ToLower());

        }

        public async Task<PagedResult<Product>> GetProductsByCategories(string? categoryName, string? categoryId, double? Price, int pageNumber, int pageSize)
        {

            var query = _db.Products.
                AsNoTracking().
                Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoryName))
                query = query.Where(p => p.Category.CategoryName.ToLower().Contains(categoryName.ToLower()));

            if (!string.IsNullOrWhiteSpace(categoryId))
                query = query.Where(p => p.Category.Id.ToString() == categoryId);

            if (Price.HasValue)
                query = query.Where(p => p.UnitPrice >= (Price - 2000) && p.UnitPrice <= (Price + 2000));


            int totalCount = await query.CountAsync();

            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();



            return new PagedResult<Product>
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Items = products

            };
        }


        public async Task UpdateProductAsync(Product product)
        {
            var update = await _db.Products.FindAsync(product.Id);
            if (update != null)

                // Copy values from detached product to the tracked entity
                _db.Entry(update).CurrentValues.SetValues(product);
        }
      


    }


}

