using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Rating;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.Domain.Models.RatingModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinalProject.DataAccess.Repository
{
    public class RatingRepository : Repository<Review>, IRatingRepository
    {
        private ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }
        public double FreeRate(string userId)
        {
            var reviewData = _context.Reviews
                .Where(c => c.FreelancerId == userId)
                .GroupBy(c => c.FreelancerId)
                .Select(g => new
                {
                    NumberOfReviews = g.Count(),
                    SumOfRates = g.Sum(c => c.Rate)
                })
                .FirstOrDefault();

            if (reviewData == null || reviewData.NumberOfReviews == 0)
            {
                return 0.0;
            }

            double averageRate = reviewData.SumOfRates / reviewData.NumberOfReviews;

            return averageRate;
        }



        public Review Create(RatingDto ratingDto ,string userId )
        {
            var NewRate = new Review
            {
                Rate = ratingDto.Rate,
                TaskCompletesPersentage = ratingDto.TaskCompletesPersentage,
                Comments = ratingDto.Comments,
                RateDate = DateTime.Now,
                ClientId = userId,
                FreelancerId = ratingDto.FreelancerId
            };
            _context.Add(NewRate);
            return NewRate;
        }

        public bool FindReview(Expression<Func<Review, bool>> predicate)
        {
            return _context.Set<Review>().Any(predicate);
        }

        public Review EditReview(int id, EditRat ratingDto)
        {
            var oldRating = _context.Reviews.Find(id);
            if(oldRating != null)
            {
                oldRating.ClientId = oldRating.ClientId;
                oldRating.FreelancerId = oldRating.FreelancerId;
                oldRating.Rate = ratingDto.Rate;
                oldRating.Comments = ratingDto.Comments;
                oldRating.RateDate = DateTime.Now;
                oldRating.TaskCompletesPersentage = ratingDto.TaskCompletesPersentage;

                return oldRating;
            }
            return null;
        }
    }
}
