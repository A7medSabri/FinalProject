using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.FavoritesTable;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<FavoritesFreelancer> FindAll(string userId)
        {
            return _context.Favorites
                           .Where(c => c.ClientId == userId)
                           .ToList();
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
    }
}
