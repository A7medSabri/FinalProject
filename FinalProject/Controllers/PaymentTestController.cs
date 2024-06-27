using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.Payment;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreatePaymentTest([FromForm] PaymentTestDto paymentTestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is not found.");
            }

            try
            {
                var result =  _unitOfWork.PayTest.create(userId, paymentTestDto);
                 _unitOfWork.Save();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Freelancer,Admin")]
        [HttpGet("Get-Freelancer-Money")]
        public async Task<IActionResult> GetMoney()
        {
            string userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is not found.");
            }

            try
            {
                var money =  _unitOfWork.PayTest.GetMyMoney(userId);
                return Ok(money);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the money.");
            }
        }

        [HttpGet("Get-All-Payments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPayments()
        {
            
                var payments = _unitOfWork.PayTest.AllPayment();
                return Ok(payments);
            

        }

    }
}
