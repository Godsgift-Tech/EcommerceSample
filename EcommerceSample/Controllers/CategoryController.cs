using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _cS;
        public CategoryController(ICategoryService cS = null)
        {
            _cS = cS;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _cS.CreateCategoryAsync(dto);
            return result.success ? Ok(result) : BadRequest(result);
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
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
        var result = await _cS.GetAllCategoriesAsync(pageNumber, pageSize);
            return  Ok(result);
        }
    }
}
