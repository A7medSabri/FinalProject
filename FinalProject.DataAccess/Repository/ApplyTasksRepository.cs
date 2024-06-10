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
            jobPost.Status = "Pending";
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
                TaskId = jp.Id,
                OrderDate = jp.OrderDate,
                DeliveryDate = jp.DeliveryDate,
                Status=jp.Status,
                TotalAmount = jp.TotalAmount,
                JobPostId=jp.JobPostId,
                ClientId=jp.ClientId,
                ClientFullName = _context.Users.FirstOrDefault(user => user.Id == jp.ClientId).FirstName + " " +
                                 _context.Users.FirstOrDefault(user => user.Id == jp.ClientId).FirstName,
                Taketitle = _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).Title,
                TakeDescription = _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).Description,
                CategoryName = _context.Categories.FirstOrDefault(category=> category.Id == _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).CategoryId).Name,
                isDeleted = jp.IsDeleted,
                isFav = (_context.FavoriteJobPost.FirstOrDefault(job=>job.FreelancerId == userId)!=null),

            }).ToList();

            return freelancerTaskDto;
        }
        public List<FreelancerTaskDto> GetAcceptedFreelancerTasksByUserId(string userId)
        {
            var applyTasks = _context.ApplyTasks
                .Where(u => u.FreelancerId == userId && u.IsDeleted == false && u.Status == "Accepted").ToList();

            if (applyTasks == null) return new List<FreelancerTaskDto>();

            var freelancerTaskDto = applyTasks.Select(jp => new FreelancerTaskDto
            {
                TaskId = jp.Id,
                OrderDate = jp.OrderDate,
                DeliveryDate = jp.DeliveryDate,
                Status = jp.Status,
                TotalAmount = jp.TotalAmount,
                JobPostId = jp.JobPostId,
                ClientId = jp.ClientId,
                ClientFullName = _context.Users.FirstOrDefault(user => user.Id == jp.ClientId).FirstName + " " +
                                 _context.Users.FirstOrDefault(user => user.Id == jp.ClientId).FirstName,
                Taketitle = _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).Title,
                TakeDescription = _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).Description,
                CategoryName = _context.Categories.FirstOrDefault(category => category.Id == _context.JobPosts.FirstOrDefault(job => job.Id == jp.JobPostId).CategoryId).Name,
                isDeleted = jp.IsDeleted,
                isFav = (_context.FavoriteJobPost.FirstOrDefault(job => job.FreelancerId == userId) != null),

            }).ToList();

            return freelancerTaskDto;
        }

        public FreelancerTaskDto GetFreelancerTaskByUserIdAndTaskId(string userId,int taskId)
        {
            var applyTask = SearchForTask(userId, taskId);
  

            if (applyTask == null) return null ;

            var freelancerTaskDto =new  FreelancerTaskDto
            {
                TaskId = applyTask.Id,
                OrderDate = applyTask.OrderDate,
                DeliveryDate = applyTask.DeliveryDate,
                Status = applyTask.Status,
                TotalAmount = applyTask.TotalAmount,
                JobPostId = applyTask.JobPostId,
                ClientId = applyTask.ClientId,
                ClientFullName = _context.Users.FirstOrDefault(user => user.Id == applyTask.ClientId).FirstName + " " +
                                 _context.Users.FirstOrDefault(user => user.Id == applyTask.ClientId).FirstName,
                Taketitle = _context.JobPosts.FirstOrDefault(job => job.Id == applyTask.JobPostId).Title,
                TakeDescription = _context.JobPosts.FirstOrDefault(job => job.Id == applyTask.JobPostId).Description,
                CategoryName = _context.Categories.FirstOrDefault(category => category.Id == _context.JobPosts.FirstOrDefault(job => job.Id == applyTask.JobPostId).CategoryId).Name,
                isDeleted = applyTask.IsDeleted,
                isFav = (_context.FavoriteJobPost.FirstOrDefault(job => job.FreelancerId == userId) != null),


            };

            return freelancerTaskDto;
        }



        public ApplyTask SearchForTask(string userId, int taskId)
        {
            var applyTasks = _context.ApplyTasks
              .FirstOrDefault(u => u.FreelancerId == userId && u.IsDeleted == false && u.Id == taskId);
            return applyTasks;

        }


        //---------------------client--------------------------

        public List<ClientTaskDto> Applicants(string userId, int jobId)
        {
            // check if client post this job
            var jobPost = _context.JobPosts
                    .Where(u => u.UserId == userId && u.IsDeleted == false)
                    .FirstOrDefault(u => u.Id == jobId);

            
            if (jobPost == null) return null;

            var applicants = _context.ApplyTasks.
               Where(j => j.ClientId == userId && j.JobPostId == jobId && j.IsDeleted == false).ToList();

            
            var applicantsWithName = applicants.Select(applicant =>new ClientTaskDto
            {
                TaskId = applicant.Id,
                JobPostId = applicant.JobPostId,
                FreelancerId = applicant.FreelancerId,
                FreelancerFullName = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).FirstName + " " + _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).LastName,
                FreelancerDescription = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).Description,
                FreelancerhourlyRate = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).HourlyRate,
                FreelancerProfilePictureUrl = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).ProfilePicture,
                Freelancertitle = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).YourTitle,
                isFavourite = (_context.Users
                .Include(u => u.Favorites)
                .FirstOrDefault(u => u.Id == userId)
                .Favorites.FirstOrDefault(u=>u.FreelancerId == applicant.FreelancerId)!=null)


            }).ToList();

          
            return applicantsWithName;
        }

        public List<ClientTaskDto> AccpetedApplicants(string userId)
        {
            // check if client post this job
            var jobPost = _context.JobPosts
                    .Where(u => u.UserId == userId && u.IsDeleted == false && u.Status == "Accepted");
         


            if (jobPost == null) return null;

            var applicants = _context.ApplyTasks.
               Where(j => j.ClientId == userId && j.IsDeleted == false).ToList();


            var applicantsWithName = applicants.Select(applicant => new ClientTaskDto
            {
                TaskId = applicant.Id,
                JobPostId = applicant.JobPostId,
                FreelancerId = applicant.FreelancerId,
                FreelancerFullName = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).FirstName + " " + _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).LastName,
                FreelancerDescription = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).Description,
                FreelancerhourlyRate = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).HourlyRate,
                FreelancerProfilePictureUrl = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).ProfilePicture,
                Freelancertitle = _context.Users.FirstOrDefault(u => u.Id == applicant.FreelancerId).YourTitle,
                isFavourite = (_context.Users
                .Include(u => u.Favorites)
                .FirstOrDefault(u => u.Id == userId)
                .Favorites.FirstOrDefault(u => u.FreelancerId == applicant.FreelancerId) != null)


            }).ToList();


            return applicantsWithName;
        }
    }
}
