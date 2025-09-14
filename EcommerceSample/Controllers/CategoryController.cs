using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Application.Common.ServiceImplementations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _cS;
        public CategoryController(ICategoryService cS )
        {
            _cS = cS;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _cS.CreateCategoryAsync(dto, userId);
            return Ok(result);
        }

        [HttpPut("update_{id}")]
        
        public async Task<IActionResult> Update( Guid id, [FromBody] UpdateCategoryDto dto)
        {
            var result = await _cS.UpdateCategoryAsync(id, dto);
            return result.success ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(Guid id)
        {
            var result = await _cS.DeleteCategoryAsync(id);
            return result.success ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult>Get(Guid id)
        {
            var result = await _cS.GetCategoryById(id);
            return result.success ? Ok(result) : NotFound(result) ;
        }

        [HttpGet("categoryName")]
        public async Task<IActionResult> Get(string name)
        {
            var result = await _cS.GetCategoryByName(name);
            return result.success ? Ok(result) : NotFound(result);
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
        var result = await _cS.GetAllCategoriesAsync(pageNumber, pageSize);
            return  Ok(result);
        }
    }
}
