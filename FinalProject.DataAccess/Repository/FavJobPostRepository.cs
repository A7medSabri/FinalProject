using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.FavoritesTable;
using FinalProject.Domain.Models.JobPostAndContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class FavJobPostRepository : Repository<JobPost>, IFavJobPostRepository
    {
        private ApplicationDbContext _context;

        public FavJobPostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public FavJobPost FindFavJobPost(Expression<Func<FavJobPost, bool>> predicate)
        {
            return _context.Set<FavJobPost>().FirstOrDefault(predicate);
        }

        public List<MyFavJobPost> FindMyFavJobPost(string Fid)
        {
            var favJobPostsList = _context.FavoriteJobPost
                .Where(u => u.FreelancerId == Fid)
                .Include(u=>u.Jobpost)
                .Select(u => new MyFavJobPost
                {
                    jobPostId=u.JobpostId,
                    jobPost = u.Jobpost.Title
                })
                .ToList();
            return favJobPostsList;
        }

        public FavJobPostDto Create(FavJobPostDto favJobDto, string userId)
        {
            var newFav = new FavJobPost();
            newFav.FreelancerId = userId;
            newFav.JobpostId = favJobDto.JobpostId;

            _context.FavoriteJobPost.Add(newFav);
            return favJobDto;
        }
        public bool Remove(int jobPost)
        {
            var favorite = _context.FavoriteJobPost.FirstOrDefault(c => c.JobpostId == jobPost);

            if (favorite != null)
            {
                _context.FavoriteJobPost.Remove(favorite);
                _context.SaveChanges();
            }
            return true;
        }

        public bool CreateFavJobPost(int JobId, string userId)
        {
            var newFav = new FavJobPost();
            newFav.FreelancerId = userId;
            newFav.JobpostId = JobId;

            _context.FavoriteJobPost.Add(newFav);
            return true;
        }

        public void Delete(FavJobPost favJobPost)
        {
            _context.Remove(favJobPost);

        }
    }
}
