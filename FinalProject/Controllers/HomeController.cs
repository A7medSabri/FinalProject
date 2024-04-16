using FinalProject.Domain.DTO.AccountModel;
using FinalProject.Domain.DTO.HomeModel;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize (Roles ="Admin , User")]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment , IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Get-All-Freelancers")]
        public async Task<IActionResult> GetAllFreelancer()
        {
            var users = await _userManager.Users
               .ToListAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No users found with the specified name.");
            }

            var FreeLancersList = new List<GetAllFreelancer>();

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            foreach (var user in users)
            {
                if(!await _userManager.IsInRoleAsync(user, "Freelancer"))
                {
                    continue;
                }
                string profilePictureFileName = user.ProfilePicture ?? "default.jpg";
                string filePath = Path.Combine(wwwRootPath, "FreeLancerProfileImage", profilePictureFileName);

                var isFreeLancer = await _userManager.IsInRoleAsync(user, "Freelancer");
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");


                if (user.Age != null && user.YourTitle != null && user.Description != null && user.ZIP != null
                    && isFreeLancer && !isAdmin)
                {
                    var freelancer = new GetAllFreelancer
                    {
                        id = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        YourTitle = user.YourTitle,
                        Description = user.Description,
                        ProfilePicture = filePath ?? " ",
                        HourlyRate = user.HourlyRate
                    };

                    FreeLancersList.Add(freelancer);
                }
            }
            if (FreeLancersList.Any())
            {
                return Ok(FreeLancersList);
            }
            else
            {
                return NotFound("No users found with the specified name.");
            }
        }

        [HttpGet("Get-Freelancer-By-ID")]
            public async Task<IActionResult> GetAllFreelancerByID(string Fid)
            {
                var user = _userManager.Users
                    .Include(i => i.UserLanguages)
                        .ThenInclude(i => i.Language)
                    .Include(i => i.UserSkills)
                        .ThenInclude(i => i.Skill)
                     .FirstOrDefault(u => u.Id == Fid);

                var result = _unitOfWork.Rating.FreeRate(Fid);


                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = user.ProfilePicture;
                string filePath = Path.Combine(wwwRootPath, @"FreeLancerProfileImage", fileName);

                var freelancer = new GetFreelancer
                {
                    id = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    YourTitle = user.YourTitle,
                    Description = user.Description,
                    SelectedLanguages = user.UserLanguages?.Select(lang => lang.Language.Value).ToList(),
                    SelectedSkills = user.UserSkills?.Select(skill => skill.Skill.Name).ToList(),
                    PortfolioURl = user.PortfolioURl,
                    ProfilePicture = filePath,
                    Address = user.State + " " + user.Address,
                    Country = user.Country,
                    HourlyRate = user.HourlyRate,
                    Rate = result
                };

                return Ok(freelancer);

            }
        
        

        [HttpGet("Get-All-Freelancer-With-The-SameName")]
        public async Task<IActionResult> GetAllFreelancerWithTheSameName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("The name parameter cannot be null or empty.");
            }

            var lowercaseName = name.ToLower();

            var users = await _userManager.Users
                .Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(lowercaseName))
                .ToListAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No users found with the specified name.");
            }

            var FreeLancersList = new List<GetAllFreelancer>();

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            foreach (var user in users)
            {

                string profilePictureFileName = user.ProfilePicture ?? "default.jpg";
                string filePath = Path.Combine(wwwRootPath, "FreeLancerProfileImage", profilePictureFileName);

                var isFreeLancer = await _userManager.IsInRoleAsync(user, "Freelancer");
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");


                if (user.Age != null && user.YourTitle != null && user.Description != null && user.ZIP != null
                    && isFreeLancer && !isAdmin)
                {
                    var freelancer = new GetAllFreelancer
                    {
                        id = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        YourTitle = user.YourTitle,
                        Description = user.Description,
                        ProfilePicture = filePath ?? " ",
                        HourlyRate = user.HourlyRate
                    };

                    FreeLancersList.Add(freelancer);
                }
            }

            if (FreeLancersList.Any())
            {
                return Ok(FreeLancersList);
            }
            else
            {
                return NotFound("No users found with the specified name.");
            }
        }

    }
}
