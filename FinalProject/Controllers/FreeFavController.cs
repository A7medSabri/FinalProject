using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
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
            var data = _unitOfWork.Favorites.FindAll(userId);
            if(data.Count == 0 )
            {
                return Ok("No Fav To Display");
            }

            return Ok(data);
        }

        [HttpDelete("Delete-Fav-Free")]
        public async Task<IActionResult> DeleteFavFree(string Fid)
        {
            var data = _unitOfWork.Favorites.Find(c => c.FreelancerId == Fid).FirstOrDefault();

            if (data != null)
            {
                _unitOfWork.Favorites.Remove(Fid);
                _unitOfWork.Save();

                return Ok("Deleted");
            }
            return NotFound("Not Found To Delete");
        }
    }


}
