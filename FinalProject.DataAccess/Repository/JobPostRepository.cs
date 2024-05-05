using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.JobPost;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.FavoritesTable;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalProject.DataAccess.Repository
{
    public class JobPostRepository : Repository<JobPost>, IJobPostRepository
    {
        ApplicationDbContext _context;
        public JobPostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }


        // related to frelacner only
        public List<GetMyJobPostDto> GetAllJobPosts(string freelancerId)
        {
            var jobPosts = _context.JobPosts
                .Where(u => u.IsDeleted == false)
                .Include(u => u.ApplicationUser)
                .Include(jp => jp.JobPostSkill)
                    .ThenInclude(u => u.Skill)
                .Include(u => u.Category)
                .ToList();

            var favJobPosts = _context.FavoriteJobPost.Where(f=> f.FreelancerId == freelancerId).ToList();

            if (jobPosts == null) return null;

            var jobPostDtos = jobPosts.Select(jobPost => new GetMyJobPostDto
            {

                Id = jobPost.Id,
                Title = jobPost.Title,
                Description = jobPost.Description,
                Price = jobPost.Price,
                DurationTime = jobPost.DurationTime,
                JobPostSkill = jobPost.JobPostSkill.Select(skill => skill.Skill.Name).ToList(),
                CategoryName = jobPost.Category.Name,
                Status = jobPost.Status,
                IsDeleted = jobPost.IsDeleted,
                UserId = jobPost.UserId,
                UserFullName = jobPost.ApplicationUser.FirstName + " " + jobPost.ApplicationUser.LastName,
                IsFav = favJobPosts.FirstOrDefault(j => j.JobpostId == jobPost.Id) != null

            }).ToList();

            return jobPostDtos;
        }

        // for client
        public GetMyJobPostDto GetjopPostWithId(string userId,int id)
        {
            var jobPost = _context.JobPosts
                .Include(u => u.Category)
                .Include(u => u.ApplicationUser)
                .Include(u => u.JobPostSkill)
                  .ThenInclude(u => u.Skill)
                .Where(u => u.UserId == userId && u.IsDeleted == false)
                .FirstOrDefault(u => u.Id == id);

            if (jobPost == null) return null;

            var jopPostDto = new GetMyJobPostDto
            {
                Id = jobPost.Id,
                Title = jobPost.Title,
                Description = jobPost.Description,
                Price = jobPost.Price,
                DurationTime = jobPost.DurationTime,
                JobPostSkill = jobPost.JobPostSkill.Select(skill => skill.Skill.Name).ToList(),
                CategoryName = jobPost.Category.Name,
                Status = jobPost.Status,
                IsDeleted = jobPost.IsDeleted,
                UserId = jobPost.UserId,
                UserFullName = jobPost.ApplicationUser.FirstName + " " + jobPost.ApplicationUser.LastName
            };
            return jopPostDto;
        }

        // related to frelacner only

        public List<AllJopPostDto> GetAllByName(string freelancerId, string tilte)
        {

            var lower = tilte.ToLower();

            var AllJopPost = _context.JobPosts
                .Where(u => u.IsDeleted == false)
                .Include(u => u.Category)
                .Include(u => u.ApplicationUser)
                .Where(u =>  u.Title.ToLower().Contains(lower)).ToList();

            if (AllJopPost == null) return null;

            var favJobPosts = _context.FavoriteJobPost.Where(f=> f.FreelancerId == freelancerId).ToList();



            var AllJopPostDto = AllJopPost.Select(jp => new AllJopPostDto
            {
                Id = jp.Id,
                Title = jp.Title,
                Description = jp.Description,
                Price = jp.Price,
                DurationTime = jp.DurationTime,
                CategoryName = jp.Category.Name,
                Status = jp.Status,
                IsDeleted = jp.IsDeleted,
                UserId = jp.UserId,
                FullNameForUser = jp.ApplicationUser.FirstName + " " + jp.ApplicationUser.LastName,
                IsFav = favJobPosts.FirstOrDefault(j => j.JobpostId == jp.Id) != null

            }).ToList();

            return AllJopPostDto;

        }
        public List<GetMyJobPostDto> GetAllJobPostsByUserId(string userId)
        {
            var jobPosts = _context.JobPosts
                .Include(jp => jp.JobPostSkill)
                    .ThenInclude(u => u.Skill)
                .Include(u => u.Category)
                .Include(u => u.ApplicationUser)
                .Where(u => u.UserId == userId && u.IsDeleted == false).ToList();

            if (jobPosts == null) return null;

            var jobPostDtos = jobPosts.Select(jp => new GetMyJobPostDto
            {
                Id = jp.Id,
                Title = jp.Title,
                Description = jp.Description,
                CategoryName = jp.Category.Name,
                Price = jp.Price,
                DurationTime = jp.DurationTime,
                JobPostSkill = jp.JobPostSkill.Select(skill => skill.Skill.Name).ToList(),
                Status = jp.Status,
                IsDeleted = jp.IsDeleted,
                UserId = jp.UserId,
                UserFullName = jp.ApplicationUser.FirstName + " " + jp.ApplicationUser.LastName

            }).ToList();

            return jobPostDtos;
        }

        public void Update(int id, JobPostDto jobPostDto)
        {
            // jobPost always exist
            JobPost NewJobPost = _context.JobPosts.FirstOrDefault(post => post.Id == id);

            NewJobPost.Title = jobPostDto.Title;
            NewJobPost.Description = jobPostDto.Description;
            NewJobPost.Price = jobPostDto.Price;
            NewJobPost.DurationTime = jobPostDto.DurationTime;
        }


        public void Create(JobPostDto jobPostDto, string UserId)
        {
            JobPost jobPost = new JobPost();

            // Use the provided userId instead of accessing User object
            jobPost.UserId = UserId;

            jobPost.Title = jobPostDto.Title;
            jobPost.Description = jobPostDto.Description;
            jobPost.Price = jobPostDto.Price;
            jobPost.DurationTime = jobPostDto.DurationTime;
            jobPost.Status = "Uncompleted";
            jobPost.CategoryId = jobPostDto.CategoryId;

            _context.JobPosts.Add(jobPost);
        }


        public JobPost GetJobPostByIdAndUserId(string userId,int id)
        {
            var jobPost = _context.JobPosts
                .Include(u => u.ApplicationUser)
                .Where(jp => jp.UserId == userId && jp.IsDeleted == false)
                .FirstOrDefault(jp => jp.Id == id);

            return jobPost;
        }


    }
}
