using AutoMapper;
using FinalProject.Domain.DTO.ApplyTasks;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Channels;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplyTasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplyTasksController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }


        //---------------------Freelancer--------------------------

        [HttpPost("Freelancer-Apply-For-Task")]
        [Authorize(Roles = "Freelancer")]
        public IActionResult ApplyForTask(int jobId)
        {
            string userId = User.FindFirst("uid")?.Value;

            // Validate the input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(userId == null)
            {
                return BadRequest("Login please.");
            }
            if (_unitOfWork.JobPost.GetByID(jobId) == null)
            {
                return NotFound("This job not exsist");
            }
               
            if(!_unitOfWork.ApplyTasks.Create(jobId, userId))
            {
                // freelancer apply for this task before
                return BadRequest("You applyied for this job before");
            }
            _unitOfWork.Save();

            return Ok("Completed Applying");
        }



        [HttpGet("Freelancer-Applied-Tasks")]
        [Authorize(Roles = "Freelancer")]
        public IActionResult GetAppliedTasks()
        {
            string userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return BadRequest("Login please.");
            }
            var myTasks = _unitOfWork.ApplyTasks.GetAllFreelancerTasksByUserId(userId);
            return Ok(myTasks);
        }

        [HttpGet("Freelancer-Accepted-Tasks")]
        [Authorize(Roles = "Freelancer")]
        public IActionResult GetAcceptedTasks()
        {
            string userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return BadRequest("Login please.");
            }
            var myTasks = _unitOfWork.ApplyTasks.GetAcceptedFreelancerTasksByUserId(userId);
            return Ok(myTasks);
        }


        // get task by id
        // GET: api/ApplyTasks/5
        [HttpGet("Freelancer-Applied-Task-By-Id")]
        [Authorize(Roles = "Freelancer")]
        public IActionResult GetAppliedTask(int taskId)
        {
            string userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return BadRequest("Login please.");
            }
            var myTask = _unitOfWork.ApplyTasks.GetFreelancerTaskByUserIdAndTaskId(userId,taskId);
            return Ok(myTask);
        }



        // put: api/ApplyTasks/5
        [Authorize(Roles = "Freelancer")]
        [HttpPut("Freelancer-Delete-Task")]
        public IActionResult DeleteTask(int taskId)
        {
            string userId = User.FindFirst("uid")?.Value;

            var Task = _unitOfWork.ApplyTasks.SearchForTask(userId, taskId);
            if (Task == null) return NotFound("This task not exsist");
            Task.IsDeleted = true;
            _unitOfWork.Save();
            return Ok("Deleted");
        }





        //---------------------Client--------------------------
        [HttpGet("Client-Applicants")]
        [Authorize(Roles = "User")]
        public IActionResult GetApplicantsForTask(int jobId)
        {
            string userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return BadRequest("Login please.");
            }
            // TODO: Retrieve the applicants for the specified task

           // "There are no applicants yet"
            var applicants = _unitOfWork.ApplyTasks.Applicants(userId,jobId);
            if (applicants == null) return NotFound("You don't post this job yet");
           
            return Ok(applicants);
        }


        [HttpGet("Get-Accept-Client- Applicants")]
        [Authorize(Roles = "User")]
        public IActionResult GetAcceptApplicants()
        {
            string userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return BadRequest("Login please.");
            }
            // TODO: Retrieve the applicants for the specified task

            // "There are no applicants yet"
            var applicants = _unitOfWork.ApplyTasks.AccpetedApplicants(userId);
            if (applicants == null) return NotFound("There are no accpeted applicatn yet");

            return Ok(applicants);
        }

        [HttpPut("Client-Accept-Applicant")]
        [Authorize(Roles = "User")]
        public IActionResult AcceptApplicantForTask(int taskId)
        {
            string userId = User.FindFirst("uid")?.Value;
            ApplyTask task = _unitOfWork.ApplyTasks.GetByID(taskId);
            if(task == null) return NotFound();
            // TODO: Accept the applicant for the specified task
            task.Status = "Accepted";

            _unitOfWork.Save();
            return Ok("Applicant accepted successfully");
        }

        [HttpPut("Client-Reject-Applicant")]
        [Authorize(Roles = "User")]
        public IActionResult RejectApplicantForTask(int taskId)
        {
            string userId = User.FindFirst("uid")?.Value;
            ApplyTask task = _unitOfWork.ApplyTasks.GetByID(taskId);
            if (task == null) return NotFound();
            // TODO: Reject the applicant for the specified task
            task.Status = "Rejected";

            _unitOfWork.Save();
            return Ok("Applicant rejected successfully");
        }

    }
}
