using FinalProject.Domain.DTO.JobPost;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IJobPostRepository : IRepository<JobPost>
    {
        // Freelancer
        public List<GetFreelancerJobPostDto> GetAllJobPosts(string freelancerId);
        public List<GetFreelancerJobPostDto> GetFreelancerJobsByName(string freelancerId, string name);


        // client
        public List<GetClientJobPostDto> GetAllJobPostsByUserId(string userId);
        void Update(int id, JobPostDto jobPostDto);
        public GetClientJobPostDto GetjopPostWithId(string userId,int id);


        void Create(JobPostDto jobPostDto,string UserId);

        public JobPost GetJobPostByIdAndUserId(string userId, int id);

    }
}
