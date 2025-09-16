using AutoMapper;
using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.Interfaces.UnitOfWork;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOFWorks _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private const string AllProductCacheKey = "all_product_cacheKey";
        public ProductService(IUnitOFWorks unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<ServiceResponse<CreateProductDto>> CreateProductAsync(CreateProductDto product, string userId)
        {
          var existingProduct = await _unitOfWork.ProductRepository.GetProductByName(product.ProductName);
            if (existingProduct != null) return new ServiceResponse<CreateProductDto>(null!, false,  $"{product.ProductName}  already exists");
            var newProduct = _mapper.Map<Product>(product);
            newProduct.UserId = userId;
            //  Ensure default if not provided
            if (string.IsNullOrEmpty(newProduct.Currency))
                newProduct.Currency = "NGN";
            await _unitOfWork.ProductRepository.CreateProductAsync(newProduct);
            await _unitOfWork.Completed();
            var createdProduct = _mapper.Map<CreateProductDto>(newProduct);

            // Cache
            _memoryCache.Set($"_{newProduct.Id}",createdProduct, TimeSpan.FromMinutes(5));
            _memoryCache.Remove(AllProductCacheKey);
            return new ServiceResponse<CreateProductDto>(createdProduct, true, "Product created successfuly");

           
        }

        public async Task<ServiceResponse<bool>> DeletProductAsync(Guid productId)
        {
            try
            {
                var result = await _unitOfWork.ProductRepository.DeleteProductAsync(productId);
                if (!result) return new ServiceResponse<bool>(false, false, "Product was not found or already deleted");
                await _unitOfWork.Completed();
                _memoryCache.Remove($"Product_{productId}");
                _memoryCache.Remove(AllProductCacheKey );
                return new ServiceResponse<bool>(result, true, "Product was successfully deleted");

            }
            catch (Exception ex)
            {

                return new ServiceResponse<bool>(false, false, $"Error encountered while deleting product:{ex.Message} ");
            }
        }

        public  async Task<ServiceResponse<PagedResult<GetProductDto>>> GetAllProductAsync(string? categoryName, string? categoryId, double? Price, int pageNumber, int pageSize)
        {

            var pagedProduct = await _unitOfWork.ProductRepository.GetProductsByCategories(categoryName, categoryId, Price, pageNumber, pageSize);
            if (pagedProduct.Items.Count == 0)
                return new ServiceResponse<PagedResult<GetProductDto>>(null!, false, "No product found matching criteria");
            var products = _mapper.Map<List<GetProductDto>>(pagedProduct.Items);

            var result = new PagedResult<GetProductDto>
            {
                TotalCount = pagedProduct.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = products 

            };
            return new ServiceResponse<PagedResult<GetProductDto>>(result, true, "Products retrieved successfuly");
        
        }

        public async Task<ServiceResponse<CreateProductDto>> GetProductById(Guid productId)
        {
            var existingProduct = await _unitOfWork.ProductRepository.GetProductById(productId);
            if (existingProduct == null) return new ServiceResponse<CreateProductDto>(null!, false, "Product was not found");
            var productInfo = _mapper.Map<CreateProductDto>(existingProduct);
            return new ServiceResponse<CreateProductDto>(productInfo, true, "Product retieved successfully");

        }

        public async Task<ServiceResponse<CreateProductDto>> GetProductByName(string productName)
        {
            var existingProduct = await _unitOfWork.ProductRepository.GetProductByName(productName);
            if (existingProduct == null) return new ServiceResponse<CreateProductDto>(null!, false, $"{productName}  product was not found");
            var productInfo = _mapper.Map<CreateProductDto>(existingProduct);
            return new ServiceResponse<CreateProductDto>(productInfo, true, $"{productName} product retieved successfully");
        }

        public async Task<ServiceResponse<ProductSummaryDto>> GetProductSummaryHistory(Guid productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductById(productId);
            if (product == null) return new ServiceResponse<ProductSummaryDto>(null!, false, "Product was not found");
            var productHistory = _mapper.Map<ProductSummaryDto>(product);
            return new ServiceResponse<ProductSummaryDto>(productHistory, true, "Product history was retrieved successfuly");
        }

        public async Task<ServiceResponse<ProductDto>> UpdateProductAsync(Guid productId, UpdateProductDto product)
        {
            var existingProduct = await _unitOfWork.ProductRepository.GetProductById(productId);
            if (existingProduct == null) return new ServiceResponse<ProductDto>(null!, false, "Product was not found");
            //Map DTO to tracked entity
            _mapper.Map(product, existingProduct);

            await _unitOfWork.ProductRepository.UpdateProductAsync(existingProduct);
            await _unitOfWork.Completed();
            var updatedProduct = _mapper.Map<ProductDto>(existingProduct);
            //  update the timestamp

            updatedProduct.UpdatedAT = DateTime.UtcNow;
            // remove cache
            _memoryCache.Remove($"product_{productId}");
            _memoryCache.Remove(AllProductCacheKey);
            return new ServiceResponse<ProductDto>(updatedProduct, true, "Product updated successfully");

           
        }
    }
}
