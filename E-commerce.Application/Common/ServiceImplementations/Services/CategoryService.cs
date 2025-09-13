using AutoMapper;
using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.Interfaces.UnitOfWork;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace E_commerce.Application.Common.ServiceImplementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOFWorks _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        private const string AllCategoryCacheKey = "all_category_cacheKey";
        public CategoryService(IUnitOFWorks unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

      

        public async Task<ServiceResponse<CreateCategoryDto>> CreateCategoryAsync(CreateCategoryDto categoryDto, string userId)
        {
            var existingCategory = await _unitOfWork.CategoryRepository
                .GetCategoryByName(categoryDto.CategoryName);

            if (existingCategory != null)
            {
                return new ServiceResponse<CreateCategoryDto>(null!, false, "This category already exists. Proceed to add products.");
            }

            var newcategory = _mapper.Map<Category>(categoryDto);
           // newcategory.UserId=user
                 newcategory.UserId = userId;
            await _unitOfWork.CategoryRepository.CreateCategoryAsync(newcategory);
            await _unitOfWork.Completed();

            var createdCategory = _mapper.Map<CreateCategoryDto>(newcategory);

            // Cache
           
            _memoryCache.Set($"Category_{newcategory.Id}", createdCategory, TimeSpan.FromMinutes(5));
            _memoryCache.Remove(AllCategoryCacheKey);
            return new ServiceResponse<CreateCategoryDto>(
                 createdCategory, true,"Category created successfully.");
        }


        public async Task<ServiceResponse<bool>> DeleteCategoryAsync(Guid categoryId)
        {
            try
            {
                var result = await _unitOfWork.CategoryRepository.DeleteCategoryAsync(categoryId);
                if (!result)
                    return new ServiceResponse<bool>(false, false, "category not found or already deleted");

                await _unitOfWork.Completed();
                _memoryCache.Remove($"Categort_{categoryId}");
                _memoryCache.Remove(AllCategoryCacheKey );
                return new ServiceResponse<bool>(true, true, "product category was removed successfully");

            }

            catch (Exception ex)
            {
              return new ServiceResponse<bool>(false, false, $" Error was encoutered in deleting product category:  { ex.Message}");
            }
        }

        public async Task<ServiceResponse<PagedResult<CategoryDto>>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            var cacheKey = $"{AllCategoryCacheKey}_page_{pageNumber}_size_{pageSize}";
            if (_memoryCache.TryGetValue(cacheKey, out PagedResult<CategoryDto>? cachedCategory))
            {
                return new ServiceResponse<PagedResult<CategoryDto>>(cachedCategory, true, "Categories loaded from cache");
            }
            var result = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync(pageNumber, pageSize);

            var mappedResult = new PagedResult<CategoryDto>
            {
              Items = _mapper.Map<List<CategoryDto>>(result),
              TotalCount = result.TotalCount,
              PageSize = result.PageSize,
               PageNumber = result.PageNumber
            };
            _memoryCache.Set(cacheKey, mappedResult, TimeSpan.FromMinutes(5));

            return new ServiceResponse<PagedResult<CategoryDto>>(mappedResult, true, "categories feltched and cached");
        }

        public async Task<ServiceResponse<CreateCategoryDto>> GetCategoryById(Guid categoryId)
        {
            var existingCategory = await _unitOfWork.CategoryRepository.GetCategoryById(categoryId);
            if (existingCategory == null) return new ServiceResponse<CreateCategoryDto>(null!, false, "category not found");
            var categoryInfo = _mapper.Map<CreateCategoryDto>(existingCategory);
            return new ServiceResponse<CreateCategoryDto>(categoryInfo, true, "Product category retrieved successfully");

        }

        public async Task<ServiceResponse<CategoryDto>> UpdateCategoryAsync(Guid categoryId, UpdateCategoryDto category)
        {
            var existingCategory = await _unitOfWork.CategoryRepository.GetCategoryById(categoryId);
            if (existingCategory == null) return new ServiceResponse<CategoryDto>(null!, false, "category not found");
            var updateCategory = _mapper.Map<UpdateCategoryDto>(existingCategory);
            await _unitOfWork.CategoryRepository.UpdateCategoryAsync(existingCategory);
            await _unitOfWork.Completed();
            var updatedCategory = _mapper.Map<CategoryDto>(existingCategory);
            // remove cache
            _memoryCache.Remove($"category_{categoryId}");
            _memoryCache.Remove(AllCategoryCacheKey);
            return new ServiceResponse<CategoryDto>(updatedCategory, true, "Product category updated ");
        }
    }
}
