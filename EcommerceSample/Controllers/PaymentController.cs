using E_commerce.Application.Common.Interfaces.ServiceInterfaces;
using E_commerce.Application.Common.ServiceImplementations.APP_ServiceResponse;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Payment;
using E_commerce.Application.Common.ServiceImplementations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _pS;
        public PaymentController(PaymentService pS)
        {
            _pS = pS;
        }
      
        [HttpGet("user")]
        public async Task<IActionResult> GetPaymentsByUser([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Extract userId from JWT claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new ServiceResponse<string>("Unauthorized user", false, "User ID missing"));

            var response = await _pS.GetPaymentsByUserAsync(userId, pageNumber, pageSize);

            if (!response.success)
                return NotFound(response);

            return Ok(response);
        }

      
        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] CreatePaymentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new ServiceResponse<string>("Unauthorized user", false, "User ID missing"));

            if (!ModelState.IsValid)
                return BadRequest(new ServiceResponse<string>("Invalid payment details", false, "Validation failed"));

            var response = await _pS.MakePaymentAsync(dto, userId);

            if (!response.success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetPaymentById), new { id = response.Data.Id }, response);
        }

      
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPaymentById(Guid id)
        {
            var response = await _pS.GetPaymentsByUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!, 1, 10);

            if (response == null || response.Data == null)
                return NotFound(new ServiceResponse<string>("Payment not found", false, "No payment with that ID"));

            return Ok(response);
        }

      
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] UpdatePaymentDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ServiceResponse<string>("Mismatched payment ID", false, "Request ID does not match payment ID"));

            var response = await _pS.UpdatePaymentAsync(dto);

            if (!response.success)
                return NotFound(response);

            return Ok(response);
        }

      
        [HttpDelete("{id:guid}")]
      //  [Authorize(Roles = "Admin,Manager")] 
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var response = await _pS.DeletePaymentAsync(id);

            if (!response.success)
                return NotFound(response);

            return Ok(response);
        }
    }
}
