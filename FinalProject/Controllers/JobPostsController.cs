using Microsoft.AspNetCore.Mvc;
using FinalProject.DTO;
using FinalProject.DataAccess.Repository;
using FinalProject.DataAccess.Data;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinalProject.Domain.DTO.JobPost;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User")]
    //[Authorize (Roles = "User , Freelancer")]
    public class JobPostsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public JobPostsController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }


        [HttpGet("Get-Over-All-Project-Posts")]
        public List<GetJobPostDto> GetAllobPosts()
        {

           // var userId = User.FindFirst("uid")?.Value;

            if (_unitOfWork.JobPost.GetAllJobPosts() == null)
                return new List<GetJobPostDto>();
            return _unitOfWork.JobPost.GetAllJobPosts().ToList();
        }




        [HttpGet("Get-All-My-Project-Post")]
        public List<GetMyJobPostDto> GetJMyobPosts()
        {

            var userId = User.FindFirst("uid")?.Value;

            if (_unitOfWork.JobPost.GetAllJobPostsByUserId(userId) == null)
                return new List<GetMyJobPostDto>();
            return _unitOfWork.JobPost.GetAllJobPostsByUserId(userId).ToList();
        }


        [HttpGet("Get-All-Project-With-Same-Title")]
        public List<AllJopPostDto> GetJMyobPostsWithSameName(string tilte)
        {
            if (_unitOfWork.JobPost.GetAllByName(tilte) == null)
                return new List<AllJopPostDto>();
            return _unitOfWork.JobPost.GetAllByName(tilte);
        }


        // get jobPost by iD
        // GET: api/JobPosts/5
        [HttpGet("Get-job-post-by-Id")]
        public IActionResult GetJobPost(int id)
        {
            var jobPost = _unitOfWork.JobPost.GetjopPostWithId(id);
            if (jobPost == null)
            {
                return NotFound();
            }

            return Ok(jobPost);
        }

        [HttpPost]
        public ActionResult<JobPost> PostJobPost(JobPostDto jobPostDto)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst("uid")?.Value;

                // delete it
                //userId = "24626311-3a61-4b63-b711-7d760bd330fa";

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
        // PUT: api/JobPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutJobPost(int id, JobPostDto jobPostDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (_unitOfWork.JobPost.GetByID(id) == null)
                return NotFound();

            // jobPost always exist
            _unitOfWork.JobPost.Update(id, jobPostDto);
            _unitOfWork.Save();
            return Ok(_unitOfWork.JobPost.GetByID(id));
        }








        // DELETE: api/JobPosts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteJobPost(int id)
        {
            JobPost jobPost = _unitOfWork.JobPost.GetByID(id);
            if (jobPost == null) return NotFound();
            jobPost.IsDeleted = true;
            _unitOfWork.Save();
            return Content("Deletion Completed");
        }

    }
}
