using AutoMapper;
using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.Interfaces.UnitOfWork;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOFWorks _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public CategoryService(IUnitOFWorks unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public Task<ServiceResponse<CreateCategoryDto>> CreateCategoryAsync(CategoryDto category)
        {
            throw new NotImplementedException();

        }

        public Task<ServiceResponse<bool>> DeleteCategoryAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<PagedResult<CategoryDto>>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<CreateCategoryDto>> GetCategoryById(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<CategoryDto>> UpdateCategoryAsync(Guid categoryId, UpdateCategoryDto category)
        {
            throw new NotImplementedException();
        }
    }
}
