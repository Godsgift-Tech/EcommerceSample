using E_commerce.Application.Common.Interfaces.RepositoryInterfaces;
using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _pS;

        public ProductController(IProductService pS)
        {
            _pS = pS;
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateProductDto product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var result = await _pS.CreateProductAsync(product, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _pS.DeletProductAsync(id);
            return result.success ? Ok(result) : NotFound(result);
        }

        [HttpGet("getAll")]

        public async Task<IActionResult> GetAll([FromQuery] string? categoryName, [FromQuery] string? categoryId,
            [FromQuery] double? Price, [FromQuery] int pageNumber =1, [FromQuery] int pageSize = 10)
        {
        var result = await _pS.GetAllProductAsync(categoryName, categoryId, Price, pageNumber, pageSize);
            return result.success? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>Get(Guid id)
        {
            var result = await _pS.GetProductById(id);
            return result.success ? Ok(result) : NotFound(result);
        }

        [HttpGet("byName")]
        public async Task<IActionResult>Get(string productName)
        {
            var result = await _pS.GetProductByName(productName);
            return result.success ? Ok(result) :NotFound(result);
        }
        [HttpGet("histroryBy{id}")]
        public async Task<IActionResult>GetProductHistory(Guid id)
        {
        var result = await _pS.GetProductSummaryHistory(id);
            return result.success ? Ok(result) : NotFound(result) ;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
        {
            var result = await _pS.UpdateProductAsync(id, dto);
            return result.success ? Ok(result) : NotFound(result);
        }
    }
}
