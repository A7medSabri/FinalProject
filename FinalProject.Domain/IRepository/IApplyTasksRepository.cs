using FinalProject.Domain.DTO.ApplyTasks;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IApplyTasksRepository :IRepository<ApplyTask>
    {
        //---------------------Freelancer--------------------------

        public bool Create(string UserId, offerApply offer);
        public List<FreelancerTaskDto> GetAllFreelancerTasksByUserId(string userId);
        public List<TaskDto> GetAcceptedFreelancerTasksByUserId(string userId);
        public FreelancerTaskDto GetFreelancerTaskByUserIdAndTaskId(string userId, int taskId);
        public ApplyTask SearchForTask(string userId, int taskId);

        //---------------------client--------------------------

        public List<ClientTaskDto> Applicants(string userId, int jobId);
        public List<TaskDto> AccpetedApplicants(string userId);
    }
}
