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
        public List<GetMyJobPostDto> GetAllJobPostsByUserId(string userId);
        void Update(int id, JobPostDto jobPostDto);
        public List<AllJopPostDto> GetAllByName(string name);
        public GetMyJobPostDto GetjopPostWithId(int id);

        bool FindFavJobPost(Expression<Func<JobPost, bool>> predicate);
        //void Create(JobPostDto jobPostDto);
        void Create(JobPostDto jobPostDto);

    }
}
