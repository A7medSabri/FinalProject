﻿using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.Models.FavoritesTable;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IFavJobPostRepository : IRepository<JobPost>
    {
        FavJobPostDto Create(FavJobPostDto favJobDto, string userId);
        bool Remove(int jobPost);
        bool FindFavJobPost(Expression<Func<FavJobPost, bool>> predicate);

    }
}
