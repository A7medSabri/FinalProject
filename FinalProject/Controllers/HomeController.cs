using FinalProject.Domain.DTO.AccountModel;
using FinalProject.Domain.DTO.HomeModel;
using FinalProject.Domain.DTO.JobPost;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , User")]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        //Done
        [HttpGet("Get-All-Freelancers")]
        public async Task<IActionResult> GetAllFreelancer()
        {
            try
            {
                var freeLancersList = new List<GetAllFreelancer>();
                var users = await _userManager.Users.ToListAsync();
                var userId = User.FindFirst("uid")?.Value;
                if (users == null || !users.Any())
                {
                    return NotFound("No Freelancer found.");
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                foreach (var user in users)
                {
                    if (!await _userManager.IsInRoleAsync(user, "Freelancer"))
                    {
                        continue;
                    }

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    string finalPath = assembly.GetName().Name;
                    int index = wwwRootPath.IndexOf(finalPath);
                    var filePath = string.IsNullOrEmpty(user.ProfilePicture) ? "" : Path.Combine(index >= 0 ? wwwRootPath.Substring(index) : "", "FreeLancerProfileImage", user.ProfilePicture);

                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    var IsFreelancerFav = _unitOfWork.Favorites.IsFavOrNot(userId, user.Id);

                    if (user.Age != null && user.YourTitle != null && user.Description != null && !isAdmin)
                    {
                        var freelancer = new GetAllFreelancer
                        {
                            id = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            YourTitle = user.YourTitle ?? string.Empty,
                            Description = user.Description ?? string.Empty,
                            ProfilePicture = filePath,
                            HourlyRate = user.HourlyRate ?? 0,
                            IsFav = IsFreelancerFav,
                            Rate = _unitOfWork.Rating?.FreeRate(user.Id) ?? 0,

                        };

                        freeLancersList.Add(freelancer);
                    }
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
            catch (Exception ex)
            {

                // _logger.LogError(ex, "An error occurred while retrieving freelancers.");

                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }

        //Done
        [AllowAnonymous]
        [HttpGet("Get-Freelancer-By-ID")]
        public async Task<IActionResult> GetAllFreelancerByID(string Fid)
        {
            if (string.IsNullOrEmpty(Fid))
            {
                return BadRequest("Freelancer ID is required.");
            }
            var userId = User.FindFirst("uid")?.Value;

            try
            {
                var user = await _userManager.Users
                    .Include(i => i.UserLanguages)
                        .ThenInclude(i => i.Language)
                    .Include(i => i.UserSkills)
                        .ThenInclude(i => i.Skill)
                    .FirstOrDefaultAsync(u => u.Id == Fid);

                if (user == null || !await _userManager.IsInRoleAsync(user, "Freelancer"))
                {
                    return NotFound("We can't find this Freelancer.");
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var IsFreelancerFav = _unitOfWork.Favorites.IsFavOrNot(userId, user.Id);
                Assembly assembly = Assembly.GetExecutingAssembly();
                string finalPath = assembly.GetName().Name;
                int index = wwwRootPath.IndexOf(finalPath);
                var filePath = string.IsNullOrEmpty(user.ProfilePicture) ? "" : Path.Combine(index >= 0 ? wwwRootPath.Substring(index) : "", "FreeLancerProfileImage", user.ProfilePicture);

                var freelancer = new GetFreelancer
                {
                    id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    YourTitle = user.YourTitle ?? string.Empty,
                    Description = user.Description ?? string.Empty,
                    SelectedLanguages = user.UserLanguages?.Select(lang => lang.Language.Value).ToList() ?? new List<string>(),
                    SelectedSkills = user.UserSkills?.Select(skill => skill.Skill.Name).ToList() ?? new List<string>(),
                    PortfolioURl = user.PortfolioURl ?? string.Empty,
                    ProfilePicture = filePath,
                    Address = $"{user.State} {user.Address}".Trim(),
                    Country = user.Country ?? string.Empty,
                    HourlyRate = user.HourlyRate ?? 0,
                    Rate = _unitOfWork.Rating?.FreeRate(Fid) ?? 0,
                    IsFav = IsFreelancerFav
                };

                return Ok(freelancer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }

        //Done
        [HttpGet("Get-All-Freelancer-With-The-SameName")]
        public async Task<IActionResult> GetAllFreelancerWithTheSameName(string? name)
        {
            var freeLancersList = new List<GetAllFreelancer>();
            List<ApplicationUser> users;
            var userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(name))
            {
                users = await _userManager.Users.ToListAsync();
            }
            else
            {
                var lowercaseName = name.ToLower();
                users = await _userManager.Users
                    .Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(lowercaseName))
                    .ToListAsync();
            }
            try
            {
                if (users == null || !users.Any())
                {
                    return NotFound("No Freelancer found.");
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                foreach (var user in users)
                {
                    if (!await _userManager.IsInRoleAsync(user, "Freelancer"))
                    {
                        continue;
                    }
                    var IsFreelancerFav = _unitOfWork.Favorites.IsFavOrNot(userId, user.Id);


                    Assembly assembly = Assembly.GetExecutingAssembly();
                    string finalPath = assembly.GetName().Name;
                    int index = wwwRootPath.IndexOf(finalPath);
                    var filePath = string.IsNullOrEmpty(user.ProfilePicture) ? "" : Path.Combine(index >= 0 ? wwwRootPath.Substring(index) : "", "FreeLancerProfileImage", user.ProfilePicture);


                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                    if (user.Age != null && user.YourTitle != null && user.Description != null && !isAdmin)
                    {
                        var freelancer = new GetAllFreelancer
                        {
                            id = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            YourTitle = user.YourTitle ?? string.Empty,
                            Description = user.Description ?? string.Empty,
                            ProfilePicture = filePath,
                            HourlyRate = user.HourlyRate ?? 0,
                            IsFav = IsFreelancerFav,
                            Rate = _unitOfWork.Rating?.FreeRate(user.Id) ?? 0,


                        };

                        freeLancersList.Add(freelancer);
                    }
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
            catch (Exception ex)
            {
                // يمكنك تسجيل الأخطاء هنا باستخدام `ex` لكتابة سجل الأخطاء.

                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }

        //Done
        [AllowAnonymous]
        [HttpGet("Get-All-Freelancers-For-Home-Page")]
        public async Task<IActionResult> GetAllFreelancerForHomePage()
        {
            try
            {
                var freeLancersList = new List<GetAllFreelancer>();
                var users = await _userManager.Users.ToListAsync();
                //var userId = User.FindFirst("uid")?.Value;
                if (users == null || !users.Any())
                {
                    return NotFound("No Freelancer found.");
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                foreach (var user in users)
                {
                    if (!await _userManager.IsInRoleAsync(user, "Freelancer"))
                    {
                        continue;
                    }

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    string finalPath = assembly.GetName().Name;
                    int index = wwwRootPath.IndexOf(finalPath);
                    var filePath = string.IsNullOrEmpty(user.ProfilePicture) ? "" : Path.Combine(index >= 0 ? wwwRootPath.Substring(index) : "", "FreeLancerProfileImage", user.ProfilePicture);


                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    //var IsFreelancerFav = _unitOfWork.Favorites.IsFavOrNot(userId, user.Id);

                    if (user.Age != null && user.YourTitle != null && user.Description != null && !isAdmin)
                    {
                        var freelancer = new GetAllFreelancer
                        {
                            id = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            YourTitle = user.YourTitle ?? string.Empty,
                            Description = user.Description ?? string.Empty,
                            ProfilePicture = filePath,
                            HourlyRate = user.HourlyRate ?? 0,
                            //IsFav = IsFreelancerFav,
                            Rate = _unitOfWork.Rating?.FreeRate(user.Id) ?? 0,

                        };

                        freeLancersList.Add(freelancer);
                    }
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
            catch (Exception ex)
            {

                // _logger.LogError(ex, "An error occurred while retrieving freelancers.");

                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }

        [AllowAnonymous]
        [HttpGet("Get-All-JopPost-For-Home-Page")]
        public IActionResult GettAllJopPost()
        {

            if (_unitOfWork.JobPost.GetAllForHome == null)
            {
                return Ok(new List<JopPostHomePage>());
            }

            var jobPosts = _unitOfWork.JobPost.GetAllForHome().ToList();
            return Ok(jobPosts);

        }

        [HttpGet("Category-Search")]
        [AllowAnonymous]
        public IActionResult SearchInCategory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Category name cannot be empty");
            }

            try
            {
                var CategoryList = _unitOfWork.Category.GetAll()
                    .Where(p => p.IsDeleted == false && p.Name.ToLower().Contains(name.ToLower()))
                    .Select(s => new
                    {
                        s.Id,
                        s.Name
                    })
                    .ToList();

                if (CategoryList == null || !CategoryList.Any())
                {
                    return NotFound("No categories found matching the criteria");
                }

                return Ok(CategoryList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        #region Fun
        //[HttpGet("Get-All-Freelancer-With-The-SameName")]
        //public async Task<IActionResult> GetAllFreelancerWithTheSameName(string name)
        //{

        //    //if (string.IsNullOrEmpty(name))
        //    //{
        //    //    return BadRequest("The name parameter cannot be null or empty.");
        //    //}
        //    //var lowercaseName = name.ToLower();

        //    //var users = await _userManager.Users
        //    //    .Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(lowercaseName))
        //    //    .ToListAsync();

        //    //if (users == null || !users.Any())
        //    //{
        //    //    return NotFound("No users found with the specified name.");
        //    //}
        //    if (string.IsNullOrEmpty(name))
        //    {
        //        var users = await _userManager.Users
        //            .ToListAsync();
        //        if (users == null || !users.Any())
        //        {
        //            return NotFound("No users found.");
        //        }
        //        return Ok(users);
        //    }
        //    else
        //    {
        //        var lowercaseName = name.ToLower();

        //        var users = await _userManager.Users
        //            .Where(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(lowercaseName))
        //            .ToListAsync();
        //        if (users == null || !users.Any())
        //        {
        //            return NotFound("No users found with the specified name.");
        //        }
        //    }
        //    var FreeLancersList = new List<GetAllFreelancer>();
        //    string wwwRootPath = _webHostEnvironment.WebRootPath;
        //    foreach (var user in users)
        //    {

        //        string profilePictureFileName = user.ProfilePicture ?? "default.jpg";
        //        string filePath = Path.Combine(wwwRootPath, "FreeLancerProfileImage", profilePictureFileName);

        //        var isFreeLancer = await _userManager.IsInRoleAsync(user, "Freelancer");
        //        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");


        //        if (user.Age != null && user.YourTitle != null && user.Description != null && user.ZIP != null
        //            && isFreeLancer && !isAdmin)
        //        {
        //            var freelancer = new GetAllFreelancer
        //            {
        //                id = user.Id,
        //                FullName = $"{user.FirstName} {user.LastName}",
        //                YourTitle = user.YourTitle,
        //                Description = user.Description,
        //                ProfilePicture = filePath ?? " ",
        //                HourlyRate = user.HourlyRate
        //            };

        //            FreeLancersList.Add(freelancer);
        //        }
        //    }

        //    if (FreeLancersList.Any())
        //    {
        //        return Ok(FreeLancersList);
        //    }
        //    else
        //    {
        //        return NotFound("No users found with the specified name.");
        //    }
        //}

        #endregion
    }
}
