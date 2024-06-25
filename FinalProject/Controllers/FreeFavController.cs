using FinalProject.Domain.DTO.Favorites;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.FavoritesTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class FreeFavController : ControllerBase
    {

        public readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public FreeFavController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
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
            //var favoriteDtos = favoritesList.Select(f => new GetAllFavFree
            //{
            //    Freelancer = f.Freelancer.FirstName + " " + f.Freelancer.LastName,
            //    FreelancerID = f.FreelancerId,
            //    Client = f.ClientId
            //}).ToList();
            var freeLancersList = new List<GetAllFavFree>();

            foreach (var favorite in favoritesList)
            {
                var FreeRateId = favorite.FreelancerId;
                var freelancer = favorite.Freelancer;

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                Assembly assembly = Assembly.GetExecutingAssembly();
                string finalPath = assembly.GetName().Name;
                int index = wwwRootPath.IndexOf(finalPath);
                var filePath = string.IsNullOrEmpty(freelancer.ProfilePicture) ? "" : Path.Combine(index >= 0 ? wwwRootPath.Substring(index) : "", "FreeLancerProfileImage", user.ProfilePicture);

                var result = _unitOfWork.Rating.FreeRate(FreeRateId);

                var favoriteDto = new GetAllFavFree
                {
                    FreelancerID = freelancer.Id,
                    FullName = $"{freelancer.FirstName} {freelancer.LastName}",
                    YourTitle = freelancer.YourTitle ?? string.Empty,
                    Description = freelancer.Description ?? string.Empty,
                    ProfilePicture = filePath,
                    HourlyRate = freelancer.HourlyRate ?? 0,
                    IsFav = true,
                    Rate = result  // هنا تحصل على التقييم
                };

                freeLancersList.Add(favoriteDto);
            }

            if (freeLancersList.Any())
            {
                return Ok(freeLancersList);
            }
            else
            {
                return NotFound("No Freelancer found.");
            }
        }

        //[HttpPost("New-Delete-Fav-Freelancer")]
        //public async Task<IActionResult> CreateAndDelete(string Fid)
        //{
        //    var userId = User.FindFirst("uid")?.Value;
        //    if (userId == null)
        //    {
        //        return Unauthorized("You must log in.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid input.");
        //    }

        //    var data = _unitOfWork.Favorites.Find(c => c.FreelancerId == Fid && c.ClientId == userId).FirstOrDefault();
        //    //var IsExist = _unitOfWork.Favorites.FindFavFreelancer(c => c.FreelancerId == Fid && c.ClientId == userId);
        //    if (data != null /*&& IsExist*/)
        //    {
        //        _unitOfWork.Favorites.Delete(data);
        //        _unitOfWork.Save();

        //        return Ok("Deleted");
        //    }

        //    var freelancer = await _userManager.FindByIdAsync(Fid);
        //    if (freelancer == null || !(await _userManager.IsInRoleAsync(freelancer, "Freelancer")))
        //    {
        //        return BadRequest("You can only fav  freelancers.");
        //    }

        //    _unitOfWork.Favorites.CreateNewFavFreelancer(Fid, userId);
        //    _unitOfWork.Save();
        //    return Ok("Added new favorite freelancer.");
        //}
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

                return Ok(new {IsFav = false});
            }

            var freelancer = await _userManager.FindByIdAsync(Fid);
            if (freelancer == null || !(await _userManager.IsInRoleAsync(freelancer, "Freelancer")))
            {
                return BadRequest("You can only fav  freelancers.");
            }

            _unitOfWork.Favorites.CreateNewFavFreelancer(Fid, userId);
            _unitOfWork.Save();
            return Ok(new {FreelancerID = Fid , IsFav = true});
        }

        //[HttpPost("New-Freelancer-Fav")]
        //public async Task<IActionResult> CreateNew([FromForm] FavFreeDto favFreeDto)
        //{
        //    var userId = User.FindFirst("uid")?.Value;
        //    if (userId == null)
        //    {
        //        return Unauthorized("You Must Login");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    var existingFavorite = _unitOfWork.Favorites.FindByClientAndFreelancer(userId, favFreeDto.FreelancerId);
        //    if (existingFavorite != null)
        //    {
        //        // عند وجود علاقة موجودة بالفعل
        //        return Ok("Favorite already exists.");
        //    }

        //    var freelancer = await _userManager.FindByIdAsync(favFreeDto.FreelancerId);
        //    if (freelancer == null || !(await _userManager.IsInRoleAsync(freelancer, "Freelancer")))
        //    {
        //        return BadRequest("You Can Only Fav Freelancer");
        //    }

        //    _unitOfWork.Favorites.Create(favFreeDto, userId);
        //    _unitOfWork.Save();

        //    return Ok(favFreeDto);
        //}

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

            var existingFavorite = _unitOfWork.Favorites.FindByClientAndFreelancer(userId, favFreeDto.FreelancerId);
            if (existingFavorite != null)
            {
                // عند وجود علاقة موجودة بالفعل
                return BadRequest("Favorite already exists.");
            }

            var freelancer = await _userManager.FindByIdAsync(favFreeDto.FreelancerId);
            if (freelancer == null || !(await _userManager.IsInRoleAsync(freelancer, "Freelancer")))
            {
                return BadRequest("You Can Only Fav Freelancer");
            }

            _unitOfWork.Favorites.Create(favFreeDto, userId);
            _unitOfWork.Save();


            return Ok(new { FreelancerID = favFreeDto.FreelancerId , IsFav = true });
        }

        //[HttpDelete("Delete-Fav-Free")]
        //public IActionResult DeleteFavFree(string Fid)
        //{
        //    var userId = User.FindFirst("uid")?.Value;

        //    var data = _unitOfWork.Favorites.Find(c => c.FreelancerId == Fid && c.ClientId ==userId).FirstOrDefault();
        //    //var IsExist = _unitOfWork.Favorites.FindFavFreelancer(c => c.FreelancerId == Fid && c.ClientId == userId);
        //    if (data != null /*&& IsExist*/)
        //    {
        //        _unitOfWork.Favorites.Delete(data);
        //        _unitOfWork.Save();

        //        return Ok("Deleted");
        //    }
        //    return NotFound("Not Found To Delete");
        //}

        [HttpDelete("Delete-Fav-Free")]
        public IActionResult DeleteFavFree(string Fid)
        {
            var userId = User.FindFirst("uid")?.Value;

            var data = _unitOfWork.Favorites.Find(c => c.FreelancerId == Fid && c.ClientId == userId).FirstOrDefault();
            if (data != null /*&& IsExist*/)
            {
                _unitOfWork.Favorites.Delete(data);
                _unitOfWork.Save();

                return Ok(new {IsFav = false});
            }
            return NotFound("Not Found To Delete");
        }
    }


}
