﻿using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.FavoritesTable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class FavoritesRepository : Repository<FavoritesFreelancer>, IFavoritesRepository
    {
        private ApplicationDbContext _context;

        public FavoritesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public FavFreeDto Create(FavFreeDto favFreeDto, string userId)
        {
            var newFav = new FavoritesFreelancer();
            newFav.ClientId = userId;
            newFav.FreelancerId = favFreeDto.FreelancerId;

            _context.Favorites.Add(newFav);
            return favFreeDto;
        }
        public bool CreateNewFavFreelancer(string Fid,string userId)
        {
            var newFav = new FavoritesFreelancer();
            newFav.ClientId = userId;
            newFav.FreelancerId = Fid;

            _context.Favorites.Add(newFav);
            return true;
        }
        public List<FavoritesFreelancer> FindAll(string userId)
        {
            var favoritesList =  _context.Favorites
                .Include(a=>a.Freelancer)
                           .Where(c => c.ClientId == userId)
                           .ToList();
            foreach (var favorite in favoritesList)
            {
                var freelancerName = favorite.Freelancer.FirstName + " " + favorite.Freelancer.LastName;
            }

            return favoritesList;

        }

        public bool FindFavFreelancer(Expression<Func<FavoritesFreelancer, bool>> predicate)
        {
            return _context.Set<FavoritesFreelancer>().Any(predicate);

        }

        public bool Remove(string Fid)
        {
            var favorite = _context.Favorites.FirstOrDefault(c => c.FreelancerId == Fid);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }
            return true;
        }

        public FavoritesFreelancer FindByClientAndFreelancer(string clientId, string freelancerId)
        {
            return _context.Favorites.FirstOrDefault(f => f.ClientId == clientId && f.FreelancerId == freelancerId);
        }

        public bool IsFavOrNot(string clientId, string freelancerId)
        {
            
                return _context.Favorites.Any(f => f.ClientId == clientId && f.FreelancerId == freelancerId);
        }
    }
}
