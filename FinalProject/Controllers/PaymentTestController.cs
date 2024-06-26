using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.Payment;
using Microsoft.AspNet.SignalR;
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
        [Authorize(Roles = "User , Admin")]
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

        [Authorize(Roles = "Freelancer , Admin")]
        [HttpGet("Get-Freelancer-Money")]
        public IActionResult GetMoney()
        {
            var userId = User.FindFirst("uid")?.Value;
            var money = _unitOfWork.PayTest.GetMyMoney(userId);

            return Ok(money);

        }




    }
}
