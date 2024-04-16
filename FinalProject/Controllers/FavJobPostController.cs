using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Freelancer")]
    public class FavJobPostController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;

        public FavJobPostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("New-JobPost-Fav")]
        public async Task<IActionResult> CreateNew([FromForm] FavJobPostDto favJobPostDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("You Must Login");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var IsExist = _unitOfWork.FavJob.FindFavJobPost(u => u.FreelancerId == userId && u.JobpostId == favJobPostDto.JobpostId);
            if(IsExist)
            {
                return BadRequest("You Can't Fav jobPost Again");
            }

            _unitOfWork.FavJob.Create(favJobPostDto, userId);
            _unitOfWork.Save();
            return Ok(favJobPostDto);
        }

        [HttpDelete("Delete-Fav-JobPost")]
        public async Task<IActionResult> DeleteFavFree(int jobId)
        {
            var userId = User.FindFirst("uid")?.Value;
            var data = _unitOfWork.FavJob.FindFavJobPost(u=>u.Id == jobId && u.FreelancerId == userId);
            if (userId == null)
            {
                return Unauthorized("You Must Login");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (data != null)
            {

                var IsExist = _unitOfWork.FavJob.FindFavJobPost(u => u.FreelancerId == userId );
                if (IsExist)
                {
                    _unitOfWork.FavJob.Remove(jobId);
                    _unitOfWork.Save();

                    return Ok("Deleted");
                }
                return BadRequest("You Can't Fav jobPost Again");
            }
            return NotFound("Not Found To Delete");
        }
    }
}
