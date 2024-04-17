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
        [HttpGet("Get-My-Fav-JobPosts")]
        public IActionResult GetMyFavJobPost()
        {
            var userId = User.FindFirst("uid")?.Value;

            var data = _unitOfWork.FavJob.FindMyFavJobPost(userId);
            return Ok(data);

        }

        [HttpPost("New-And-Delete-JobPost-Fav")]
        public async Task<IActionResult> CreateNew(int jobId)
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
            if (_unitOfWork.JobPost.GetByID(jobId) == null)
            {
                return NotFound("No Job Post With this id");
            }
            var favJobPost = _unitOfWork.FavJob.FindFavJobPost(u => u.FreelancerId == userId && u.JobpostId == jobId);
            if (favJobPost != null)
            {
                _unitOfWork.FavJob.Delete(favJobPost);
                _unitOfWork.Save();

                return Ok("Deleted");
            }

            _unitOfWork.FavJob.CreateFavJobPost(jobId, userId);
            _unitOfWork.Save();
            return Ok(jobId);
        }

        [HttpDelete("Delete-Fav-JobPost-only")]
        public async Task<IActionResult> DeleteFavFree2(int jobId)
        {
            var userId = User.FindFirst("uid")?.Value;
            var data = _unitOfWork.FavJob.FindFavJobPost(u => u.Id == jobId && u.FreelancerId == userId);
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

                var favJobPost = _unitOfWork.FavJob.FindFavJobPost(u => u.FreelancerId == userId && u.JobpostId == jobId);
                if (favJobPost != null)
                {
                    _unitOfWork.FavJob.Delete(favJobPost);
                    _unitOfWork.Save();

                    return Ok("Deleted");
                }
                return BadRequest("You Didn't Fav This JobPost Befor");
            }
            return NotFound("Not Found To Delete");
        }

        [HttpPost("New-JobPost-Fav-only")]
        public async Task<IActionResult> CreateNew2(int jobId)
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
            if (_unitOfWork.JobPost.GetByID(jobId) == null)
            {
                return NotFound("No Job Post With this id");
            }
            var favJobPost = _unitOfWork.FavJob.FindFavJobPost(u => u.FreelancerId == userId && u.JobpostId == jobId);
            if(favJobPost !=null)
            {
                return Ok("You Fav This Befor");
            }
            _unitOfWork.FavJob.CreateFavJobPost(jobId, userId);
            _unitOfWork.Save();
            return Ok(jobId);
        }

        //[HttpPost("New-JobPost-Fav")]
        //public async Task<IActionResult> CreateNew2([FromForm] FavJobPostDto favJobPostDto)
        //{
        //    var userId = User.FindFirst("uid")?.Value;
        //    if (userId == null)
        //    {
        //        return Unauthorized("You Must Login");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    if (_unitOfWork.JobPost.GetByID(favJobPostDto.JobpostId) == null)
        //    {
        //        return NotFound("No Job Post With this id");
        //    }
        //    var IsExist = _unitOfWork.FavJob.FindFavJobPost(u => u.FreelancerId == userId && u.JobpostId == favJobPostDto.JobpostId);
        //    if (IsExist)
        //    {
        //        return BadRequest("You Can't Fav jobPost Again");
        //    }

        //    _unitOfWork.FavJob.Create(favJobPostDto, userId);
        //    _unitOfWork.Save();
        //    return Ok(favJobPostDto);
        //}
    }
}
