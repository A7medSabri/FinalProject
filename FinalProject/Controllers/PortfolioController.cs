using FinalProject.Domain.DTO.JobPost;
using FinalProject.Domain.DTO.Portfolio;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ProtfolioModle;
using FinalProject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PortfolioController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        [HttpGet("My-Protfolio")]
        public List<ProtfolioGetDto> GetMyProtfolio()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (_unitOfWork.Portfolio.GetMyPortfolio(userId) == null)
            {
                return new List<ProtfolioGetDto>();
            }

            return _unitOfWork.Portfolio.GetMyPortfolio(userId).ToList();
        }

        [HttpPut("Edit-Portfolio")]
        public async Task<IActionResult> EditPortfolio(int id, [FromBody] portfolioDto portfolioDto)
        {
            if (portfolioDto == null)
            {
                return BadRequest("Invalid request.");
            }

            var userId = User.FindFirst("uid")?.Value;

            var portfolio = await _unitOfWork.Portfolio.GetByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound($"Portfolio with ID {id} not found.");
            }


            await _unitOfWork.Portfolio.EditPortfolio(id, portfolioDto);
            _unitOfWork.Save();

            return Ok("Portfolio updated successfully.");
        }

        [HttpGet("Get-Portfolio-By-Id")]
        public async Task<IActionResult> GetPortfolioById(int id)
        {
            var portfolioDto = await _unitOfWork.Portfolio.GetByIdAsync(id);

            // التحقق من وجود المحفظة
            if (portfolioDto == null)
            {
                return NotFound($"Portfolio with ID {id} not found.");
            }

            // إعادة بيانات المحفظة على هيئة `PortfolioDto`
            return Ok(portfolioDto);
        }

        [HttpPost("Add-Portfolio")]
            public async Task<IActionResult> AddPortfolio([FromForm] AddPortfolio portfolioDto , IFormFile file)
            {
                if (ModelState.IsValid)
                {

                    var userId = User.FindFirst("uid")?.Value;

                    if (userId != null)
                    {
                        _unitOfWork.Portfolio.AddPortfolioAsync(portfolioDto , file ,userId);
                        _unitOfWork.Save();
                    return Ok(portfolioDto);
                    }
                    else
                    {
                        return BadRequest("User ID not found.");
                    }
                }

                return BadRequest(ModelState);
            }

        [HttpDelete("Delete-Portfolio")]
        public async Task<IActionResult> DeletePortfolio(int id)
        {
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            var portfolio = await _unitOfWork.Portfolio.GetByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound($"Portfolio with ID {id} not found.");
            }

            if (portfolio.UserId != userId)
            {
                return Unauthorized("User does not have permission to delete this portfolio.");
            }

            var result = await _unitOfWork.Portfolio.DeletePortfolioAsync(id);
            if (!result)
            {
                return BadRequest("Failed to delete the portfolio.");
            }

            _unitOfWork.Save();
            return Ok("Portfolio deleted successfully.");
        }


    }
}

