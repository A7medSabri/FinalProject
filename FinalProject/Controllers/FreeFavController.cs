using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.FavoritesTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class FreeFavController : ControllerBase
    {

        public readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public FreeFavController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet("Get-My-Fav-Free")]
        public IActionResult GetMyFavFree()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("You Must Login");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var favoritesList = _unitOfWork.Favorites.FindAll(userId);
            if(favoritesList.Count == 0 )
            {
                return Ok("No Fav To Display");
            }
            var favoriteDtos = favoritesList.Select(f => new GetAllFavFree
            {
                Freelancer = f.Freelancer.FirstName + " " + f.Freelancer.LastName,
                Client = f.ClientId
            }).ToList();

            return Ok(favoriteDtos);
        }

        [HttpPost("New-Delete-Fav-Freelancer")]
        public async Task<IActionResult> CreateAndDelete(string Fid)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("You must log in.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input.");
            }

            var data = _unitOfWork.Favorites.Find(c => c.FreelancerId == Fid && c.ClientId == userId).FirstOrDefault();
            //var IsExist = _unitOfWork.Favorites.FindFavFreelancer(c => c.FreelancerId == Fid && c.ClientId == userId);
            if (data != null /*&& IsExist*/)
            {
                _unitOfWork.Favorites.Delete(data);
                _unitOfWork.Save();

                return Ok("Deleted");
            }

            var freelancer = await _userManager.FindByIdAsync(Fid);
            if (freelancer == null || !(await _userManager.IsInRoleAsync(freelancer, "Freelancer")))
            {
                return BadRequest("You can only fav  freelancers.");
            }

            _unitOfWork.Favorites.CreateNewFavFreelancer(Fid, userId);
            _unitOfWork.Save();
            return Ok("Added new favorite freelancer.");
        }

        [HttpPost("New-Freelancer-Fav")]
        public async Task<IActionResult> CreateNew([FromForm] FavFreeDto favFreeDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized("You Must Login");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var freelancer = await _userManager.FindByIdAsync(favFreeDto.FreelancerId);
            if (freelancer == null || !(await _userManager.IsInRoleAsync(freelancer, "Freelancer")))
            {
                return BadRequest("Yon Can Only Fav Freelancer");
            }

            _unitOfWork.Favorites.Create(favFreeDto, userId);
            _unitOfWork.Save();

            return Ok(favFreeDto);
        }


        [HttpDelete("Delete-Fav-Free")]
        public IActionResult DeleteFavFree(string Fid)
        {
            var userId = User.FindFirst("uid")?.Value;

            var data = _unitOfWork.Favorites.Find(c => c.FreelancerId == Fid && c.ClientId ==userId).FirstOrDefault();
            //var IsExist = _unitOfWork.Favorites.FindFavFreelancer(c => c.FreelancerId == Fid && c.ClientId == userId);
            if (data != null /*&& IsExist*/)
            {
                _unitOfWork.Favorites.Delete(data);
                _unitOfWork.Save();

                return Ok("Deleted");
            }
            return NotFound("Not Found To Delete");
        }
    }


}
