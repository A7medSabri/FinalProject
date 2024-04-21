using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.ApplyTasks;
using FinalProject.Domain.DTO.JobPost;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class ApplyTasksRepository : Repository<ApplyTask>, IApplyTasksRepository
    {
        ApplicationDbContext _context;

        public ApplyTasksRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Create(ApplicationDto applicationDto, string UserId)
        {
            ApplyTask applyTask = new ApplyTask
            {

                OrderDate = DateTime.Now,
                DeliveryDate = applicationDto.DeliveryDate,
                Status = "Pending",
                TotalAmount = applicationDto.TotalAmount,
                IsDeleted = false,
                JobPostId = applicationDto.JobPostId,
                FreelancerId = UserId,
                ClientId = _context.JobPosts.FirstOrDefault(j => j.Id == applicationDto.JobPostId).UserId
            };

            _context.ApplyTasks.Add(applyTask);
        }



        public List<FreelancerTaskDto> GetAllFreelancerTasksByUserId (string userId)
        {
            var applyTasks = _context.ApplyTasks
                .Where(u => u.FreelancerId == userId && u.IsDeleted == false).ToList();

            if (applyTasks == null) return new List<FreelancerTaskDto>();

            var freelancerTaskDto = applyTasks.Select(jp => new FreelancerTaskDto
            {
                Id = jp.Id,
                OrderDate = jp.OrderDate,
                DeliveryDate = jp.DeliveryDate,
                Status=jp.Status,
                TotalAmount = jp.TotalAmount,
                JobPostId=jp.JobPostId,
                ClientId=jp.ClientId

            }).ToList();

            return freelancerTaskDto;
        }

        public FreelancerTaskDto GetFreelancerTaskByUserIdAndTaskId(string userId,int taskId)
        {
            var applyTasks = _context.ApplyTasks
                .FirstOrDefault(u => u.FreelancerId == userId && u.IsDeleted == false && u.Id == taskId);

            if (applyTasks == null) return null ;

            var freelancerTaskDto =new  FreelancerTaskDto
            {
                Id = applyTasks.Id,
                OrderDate = applyTasks.OrderDate,
                DeliveryDate = applyTasks.DeliveryDate,
                Status = applyTasks.Status,
                TotalAmount = applyTasks.TotalAmount,
                JobPostId = applyTasks.JobPostId,
                ClientId = applyTasks.ClientId

            };

            return freelancerTaskDto;
        }
    }
}
