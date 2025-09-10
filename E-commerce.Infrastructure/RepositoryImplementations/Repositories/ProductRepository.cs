using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
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
        public async Task<bool> DeleteProductAsync(Product product)
        {
          
           var getProduct = await _db.Products.FirstOrDefaultAsync(p=>p.Id == product.Id);
            if (getProduct != null)  _db.Products.Remove(product);
            return true;

        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _db.Products.ToListAsync();
       
        public async Task<Product?> GetProductById(Guid id) => await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task UpdateProductAsync(Product product)
        {
            var update = await _db.Products.FirstOrDefaultAsync(p=>p.Id==product.Id);
            if (update != null)
           _db.Products.Update(product);

        }

       
    }
}
