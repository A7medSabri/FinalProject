using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentTestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Add-New-Payment")]
        public IActionResult CreatePaymentTest([FromForm] PaymentTestDto paymentTestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the user ID retrieval is correct and secure
            string userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is not found.");
            }

            // Use consistent naming conventions for your Unit of Work methods
            var result = _unitOfWork.PayTest.create(userId, paymentTestDto);
            _unitOfWork.Save(); // Make sure this matches your Unit of Work implementation

            return Ok(result);
        }
    }
}
