﻿using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.Models.FavoritesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IFavoritesRepository : IRepository<FavoritesFreelancer>
    {
        FavFreeDto Create(FavFreeDto favFreeDto, string userId);
        List<FavoritesFreelancer> FindAll(string userId);
        bool Remove(string Fid);
        bool CreateNewFavFreelancer(string Fid, string userId);
        bool FindFavFreelancer(Expression<Func<FavoritesFreelancer, bool>> predicate);
        public FavoritesFreelancer FindByClientAndFreelancer(string clientId, string freelancerId);
        public bool IsFavOrNot(string clientId, string freelancerId);


    }
}
