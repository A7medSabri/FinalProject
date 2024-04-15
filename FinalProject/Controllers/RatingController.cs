using FinalProject.Domain.DTO.Rating;
using FinalProject.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public RatingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("Get-FreeRate-By-Id-For-Profile")]
        public IActionResult FreelancerRateById(string UserId)
        {
            var result = _unitOfWork.Rating.FreeRate(UserId);
            return Ok(result);
        }

        [Authorize(Roles ="Freelancer")]
        [HttpGet("Freelancer-Rate")]
        public IActionResult FreelancerRate()
        {
            var userId = User.FindFirst("uid")?.Value;
             var result = _unitOfWork.Rating.FreeRate(userId);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("Add-New-Rating")]
        public IActionResult ReviewFreelancer([FromForm] RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst("uid")?.Value;
            if (userId != null)
            {

                var contractExists = _unitOfWork.Contract
                      .FindContract(c => c.ClientId == userId && c.FreelancerId == ratingDto.FreelancerId && c.IsDeleted == false);

                if (!contractExists)
                {
                    return BadRequest("No contract found between the user and freelancer.");
                }

                var IsExisting = _unitOfWork.Rating.FindReview(c => c.ClientId == userId && c.FreelancerId == ratingDto.FreelancerId);

                if (IsExisting)
                {
                    return BadRequest("You Can't Rating This FreeLancer Again");
                }
                _unitOfWork.Rating.Create(ratingDto, userId);
                _unitOfWork.Save();
                return Ok(ratingDto);
            }

            return Unauthorized();
        }

        [Authorize(Roles = "User")]
        [HttpPut("Edit-Rating")]
        public IActionResult EditRating([FromForm]EditRat ratingDto , int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst("uid")?.Value;
            var data = _unitOfWork.Rating.GetByID(id);
            if (data == null)
            {
                return NotFound("No Rating");
            }
            if (userId != null)
            {
                var IsExisting = _unitOfWork.Rating.FindReview(c => c.ClientId == userId && c.FreelancerId == data.FreelancerId);
                if (IsExisting)
                {
                    _unitOfWork.Rating.EditReview(id, ratingDto);
                    _unitOfWork.Save();

                    return Ok(ratingDto);
                }

            }
            return NotFound("You Can't ReRating This Freelancer");
        }




    }
}

