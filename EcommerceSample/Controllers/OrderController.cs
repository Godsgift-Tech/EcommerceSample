using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.Pagination;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Order;
using E_commerce.Application.Common.ServiceImplementations.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _oR;
        public OrderController(IOrderService oR)
        {
            _oR = oR;
        }


        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // inject authenticated userId into dto
            dto.UserId = userId;

            var result = await _oR.CreateOrderAsync(dto);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult>Delete(Guid id)
        {
            var result = await _oR.DeleteOrderAsync(id);
            return result.success ? Ok(result) : NotFound(result);

        }

      
        [HttpGet("allOrders")]
        public async Task<IActionResult> GetAll([FromQuery] string? userId, [FromQuery] int pageNumber =1, [FromQuery] int pageSize = 10)
        {
        var result = await _oR.GetAllOrdersAsync(userId, pageNumber, pageSize);
            return result.success ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult>GetOrder(Guid id)
        {
            var result = await _oR.GetOrderByIdAsync(id);
            return result.success ? Ok(result): NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task <IActionResult> Update(Guid id, [FromBody] UpdateOrderDto dto)
        {
            var result = await _oR.UpdateOrderAsync(id, dto);
            return result.success ? Ok(result) : NotFound(result);

        }
    }
}
