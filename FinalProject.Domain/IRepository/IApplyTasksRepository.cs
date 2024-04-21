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
        public void Create(ApplicationDto applicationDto, string UserId);
        public List<FreelancerTaskDto> GetAllFreelancerTasksByUserId(string userId);
        public FreelancerTaskDto GetFreelancerTaskByUserIdAndTaskId(string userId, int taskId);
    }
}
