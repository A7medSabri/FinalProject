using Microsoft.AspNetCore.Mvc;
using FinalProject.DTO;
using FinalProject.DataAccess.Repository;
using FinalProject.DataAccess.Data;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinalProject.Domain.DTO.JobPost;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize (Roles = "User , Freelancer")]
    public class JobPostsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public JobPostsController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Freelancer , Admin")]
        [HttpGet("Get-All-Project-Posts")]
        public IActionResult GetAlljobPosts()
        {

            if (_unitOfWork.JobPost.GetAllJobPosts() == null)
            {
                return Ok(new List<GetFreelancerJobPostDto>());
            }

            var jobPosts = _unitOfWork.JobPost.GetAllJobPosts().ToList();
            return Ok(jobPosts);
        }


        [Authorize(Roles = "Freelancer")]
        [HttpGet("Get-Over-All-Project-Posts")]
        public IActionResult GetAllobPosts()
        {
            var freelancerId = User.FindFirst("uid")?.Value;

            if (_unitOfWork.JobPost.GetAllJobPosts(freelancerId) == null)
            {
                return Ok(new List<GetFreelancerJobPostDto>());
            }

            var jobPosts = _unitOfWork.JobPost.GetAllJobPosts(freelancerId).ToList();
            return Ok(jobPosts);
        }


        [Authorize(Roles = "Freelancer")]
        [HttpGet("Get-All-Project-With-Same-Title")]
        public IActionResult GetJMyobPostsWithSameName(string? title)
        {
            if (string.IsNullOrEmpty(title)) {
                return GetAllobPosts();
            }
            var freelancerId = User.FindFirst("uid")?.Value;

            var jopPostsWithSameName = _unitOfWork.JobPost.GetFreelancerJobsByName(freelancerId,title);

            if (jopPostsWithSameName == null || jopPostsWithSameName.Count == 0)
            {
                return NotFound("NO jobs found by title " + title);
            }

            return Ok(jopPostsWithSameName);
        }








        [Authorize(Roles = "User")]
        [HttpGet("Get-All-My-Project-Post")]
        public IActionResult GetClientJobPosts()
        {
            var userId = User.FindFirst("uid")?.Value;

            var myJobPosts = _unitOfWork.JobPost.GetAllJobPostsByUserId(userId).ToList();

            if (myJobPosts == null || myJobPosts.Count == 0)
            {
                return NotFound();
            }

            return Ok(myJobPosts);
        }



        // get jobPost by iD
        // GET: api/JobPosts/5
        [Authorize(Roles = "User")]
        [HttpGet("Get-job-post-by-Id")]
        public IActionResult GetJobPost(int id)
        {
            string userId = User.FindFirst("uid")?.Value;

            var jobPost = _unitOfWork.JobPost.GetjopPostWithId(userId,id);
            if (jobPost == null)
            {
                return NotFound();
            }

            return Ok(jobPost);
        }

        [Authorize(Roles = "User")]
        [HttpPost("Cliend  PostJob")]
        public IActionResult PostJobPost(JobPostDto jobPostDto)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst("uid")?.Value;

                if (userId != null)
                {
                    Console.WriteLine(userId);
                    _unitOfWork.JobPost.Create(jobPostDto, userId);
                    _unitOfWork.Save();

                    return Ok(jobPostDto);
                }
                else
                {
                    return BadRequest("User ID not found.");
                }
            }

            return BadRequest(ModelState);
        }

        // update jobpost
        [Authorize(Roles = "User")]
        [HttpPut("update jobpost")]
        public IActionResult PutJobPost(int id, JobPostDto jobPostDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            string userId = User.FindFirst("uid")?.Value;
            if (_unitOfWork.JobPost.GetJobPostByIdAndUserId(userId,id) == null)
                return NotFound();

            // jobPost always exist
            _unitOfWork.JobPost.Update(id, jobPostDto);
            _unitOfWork.Save();
            return Ok();
        }




        // DELETE: api/JobPosts/5
        [Authorize(Roles = "User")]
        [HttpPut ("Cliend delete job")]
        public IActionResult DeleteJobPost(int jobId)
        {
            string userId = User.FindFirst("uid")?.Value;
            
            JobPost jobPost = _unitOfWork.JobPost.GetJobPostByIdAndUserId(userId,jobId);
            
            if (jobPost == null) return NotFound();
            
            jobPost.IsDeleted = true;

            
            _unitOfWork.JobPost.DeleteJobPostRelatedTasks(jobId);
            
            _unitOfWork.Save();
            
            return Ok();
        }

    }
}
