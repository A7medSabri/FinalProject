using FinalProject.Domain.DTO.Rating;
using FinalProject.Domain.Models.JobPostAndContract;
using FinalProject.Domain.Models.RatingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IRatingRepository : IRepository<Review>
    {
        Review Create(RatingDto ratingDto, string userId);
        bool FindReview(Expression<Func<Review, bool>> predicate);
        Review EditReview(int id, EditRat ratingDto);
        double FreeRate(string userId);


    }
}
