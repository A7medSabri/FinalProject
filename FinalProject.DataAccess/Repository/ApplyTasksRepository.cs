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


        //---------------------Freelancer--------------------------

        public bool Create(int jobId, string UserId)
        {
            // freelancer apply for this task before
            var applyTasks = _context.ApplyTasks.
                FirstOrDefault(u => u.FreelancerId == UserId && u.JobPostId == jobId && u.IsDeleted == false);
            if (applyTasks != null) return false;

            JobPost jobPost = _context.JobPosts.FirstOrDefault(post => post.Id == jobId);
            ApplyTask applyTask = new ApplyTask
            {
                OrderDate = DateTime.Now,
                DeliveryDate = jobPost.DurationTime,
                Status = "Pending",
                TotalAmount = jobPost.Price,
                IsDeleted = false,
                JobPostId = jobId,
                FreelancerId = UserId,
                ClientId = _context.JobPosts.FirstOrDefault(j => j.Id == jobId).UserId
            };
            _context.ApplyTasks.Add(applyTask);
            return true;
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
                ClientId=jp.ClientId,
                ClientFullName = _context.Users.FirstOrDefault(user => user.Id == jp.ClientId).FirstName + " " +
                                 _context.Users.FirstOrDefault(user => user.Id == jp.ClientId).FirstName,
                title = _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).Title,
                Description = _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).Description,
                CategoryName = _context.Categories.FirstOrDefault(category=> category.Id == _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).CategoryId).Name,
                isDeleted = jp.IsDeleted,
                isFav = (_context.FavoriteJobPost.FirstOrDefault(job=>job.FreelancerId == userId)!=null),
                skills = _context.JobPosts
                          .Include(u => u.JobPostSkill)
                            .ThenInclude(u => u.Skill)
                          .FirstOrDefault(job => job.Id == jp.JobPostId)
                          .JobPostSkill
                          .Select(skill => skill.Skill.Name).ToList(),


            }).ToList();

            return freelancerTaskDto;
        }
        public ApplyTask SearchForTask(string userId, int taskId)
        {
            var applyTasks = _context.ApplyTasks
              .FirstOrDefault(u => u.FreelancerId == userId && u.IsDeleted == false && u.Id == taskId);
            return applyTasks;

        }
        public FreelancerTaskDto GetFreelancerTaskByUserIdAndTaskId(string userId,int taskId)
        {
            var applyTasks = SearchForTask(userId, taskId);

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

       
        //---------------------client--------------------------

        public List<Applicant> Applicants(string userId, int jobId)
        {
            // check if client post this job
            var jobPost = _context.JobPosts
                    .Where(u => u.UserId == userId && u.IsDeleted == false)
                    .FirstOrDefault(u => u.Id == jobId);

            
            if (jobPost == null) return null;

            var applicants = _context.ApplyTasks.
               Where(j => j.ClientId == userId && j.JobPostId == jobId && j.IsDeleted == false).ToList();

            
            var applicantsWithName = applicants.Select(applicant =>new Applicant
            {
                Id = applicant.FreelancerId,
                Name = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).FirstName + " " + _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).LastName,
                Description = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).Description,
                hourlyRate = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).HourlyRate,
                ProfilePictureUrl = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).ProfilePicture,
                Title = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).YourTitle,
                isFavourite = (_context.Users
                .Include(u => u.Favorites)
                .FirstOrDefault(u => u.Id == userId)
                .Favorites.FirstOrDefault(u=>u.FreelancerId == applicant.FreelancerId)!=null)

            }).ToList();

          
            return applicantsWithName;
        }
    }
}
