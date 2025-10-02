using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.Interfaces.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(Guid id);
        Task<Product?>GetProductById(Guid id);
        Task<Product?> GetProductByName(string productName);
        Task<PagedResult<Product>> GetProductsByCategories(string? categoryName, string? categoryId, double? Price, int pageNumber, int pageSize);
        //void Detach(Product product);
        //void AttachAsUnchanged(Product product);


    }
}
