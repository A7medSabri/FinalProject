using AutoMapper;
using FinalProject.Domain.DTO.ApplyTasks;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

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

        // get all tasks
        // GET: api/ApplyTasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskDto>> GetApplyTasks()
        {
            IEnumerable<ApplyTask> applyTasks = _unitOfWork.ApplyTasks.GetAll().Where(p => p.IsDeleted == false);
            IEnumerable<TaskDto> applyTasksDto= _mapper.Map<IEnumerable<TaskDto>>(applyTasks);

            return applyTasksDto.ToList();
        }

        // get task by id
        // GET: api/ApplyTasks/5
        [HttpGet("{id}")]
        public ActionResult<TaskDto> GetApplyTask(int id)
        {
           
            ApplyTask applyTask = _unitOfWork.ApplyTasks.GetByID(id);

            if (applyTask == null)
            {
                return NotFound();
            }
            TaskDto applyTaskDto = _mapper.Map<TaskDto>(applyTask);
            
            return applyTaskDto;
        }



        // POST: api/ApplyTasks
        [HttpPost]
        public ActionResult<ApplyTaskDto> PostApplyTask(ApplyTaskDto applyTaskDto)
        {
            //valid JobPostId 5 or 7
            //valid ClientId 24626311-3a61-4b63-b711-7d760bd330fa

            ApplyTask applyTask = _mapper.Map<ApplyTask>(applyTaskDto);


            applyTask.FreelancerId = User.FindFirst("uid")?.Value;
            // Set default OrderDate to DateTime.Now
            applyTask.OrderDate = DateTime.Now;
            applyTask.Status = "Pending";

            _unitOfWork.ApplyTasks.Add(applyTask);
            _unitOfWork.Save();

            return Ok(applyTaskDto); 
        }



        // PUT: api/ApplyTasks/5
        [HttpPut("{id}")]
        public IActionResult PutApplyTask(int id, UpdateTask updateTask)
        {
            if (!ModelState.IsValid) return BadRequest();

            ApplyTask curTask = _unitOfWork.ApplyTasks.GetByID(id);
            if (curTask == null) return NotFound();
            _mapper.Map(updateTask,curTask);
            _unitOfWork.Save();
            return Ok(GetApplyTask(id)) ;
        }

        // DELETE: api/ApplyTasks/5
        [HttpDelete("{id}")]
        public IActionResult DeleteApplyTask(int id)
        {
            ApplyTask applyTask = _unitOfWork.ApplyTasks.GetByID(id);

            if (applyTask == null)
            {
                return NotFound();
            }

            applyTask.IsDeleted = true;
            _unitOfWork.Save();

            return NoContent();
        }

        private bool ApplyTaskExists(int id)
        {
            return _unitOfWork.ApplyTasks.GetByID(id)!=null;
        }
    }
}
