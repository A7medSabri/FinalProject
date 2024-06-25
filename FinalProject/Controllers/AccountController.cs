using FinalProject.Domain.AccountModel;
using FinalProject.Domain.DTO.AccountModel;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.RegisterNeeded;
using FinalProject.Domain.Models.SkillAndCat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        //Profile User

        [HttpGet("User-Account")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return BadRequest("User ID not found in claims");
            }

            // Find the user by ID

            var user = await _userManager.FindByNameAsync(userId);

            var userProfileDto = new UserProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Country = user.Country
            };

            // Return the user profile DTO
            return Ok(userProfileDto);
        }
        //Profile Freelancer
        [HttpGet("Freelancer-Account")]
        [Authorize(Roles = "Freelancer , Admin")]
        public async Task<IActionResult> FreelancerProfile()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId2 = User.FindFirst("uid")?.Value;

            if (userId == null || userId2 == null)
            {
                return BadRequest("User ID not found in claims");
            }


            //            var user = await _userManager.FindByNameAsync(userId);
            var user = await _userManager.Users
                .Include(u => u.UserSkills)
                    .ThenInclude(u => u.Skill)
                .Include(u => u.UserLanguages)
                    .ThenInclude(u => u.Language)
                .SingleOrDefaultAsync(u => u.UserName == userId);

            var result = _unitOfWork.Rating?.FreeRate(userId2) ?? 0;

            if (user == null)
            {
                return NotFound("User not found");
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string finalPath = assembly.GetName().Name;
            int index = wwwRootPath.IndexOf(finalPath);
            var freelancerProfileDto = new FreelancerProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Rate = result,
                SelectedLanguages = user.UserLanguages?.Select(lang => lang.Language.Value).ToList() ?? new List<string>(),
                PhoneNumber = user.PhoneNumber,
                Age = user.Age,
                YourTitle = user.YourTitle,
                Description = user.Description,
                Education = user.Education,
                Experience = user.Experience,
                SelectedSkills = user.UserSkills?.Select(skill => skill.Skill.Name).ToList() ?? new List<string>(),
                HourlyRate = user.HourlyRate,
                ZIP = user.ZIP,
                Address = user.Address,
                State = user.State,
                country = user.Country,
                PortfolioURl = user.PortfolioURl,
                ProfilePicture = string.IsNullOrEmpty(user.ProfilePicture) ? "" : Path.Combine(index >= 0 ? wwwRootPath.Substring(index) : "", "FreeLancerProfileImage", user.ProfilePicture)
            };

            return Ok(freelancerProfileDto);
        }

        //Password
        [HttpPost("ChangePassword-All")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            // Extract user ID claim
            var userIdClaim = User.FindFirst("uid");
            Console.WriteLine(userIdClaim);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            if (user == null)
            {
                return BadRequest("Unable to find user");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (changePasswordResult.Succeeded)
            {
                return Ok("Password changed successfully");
            }
            else
            {
                return BadRequest("Failed to change password");
            }
        }
        //ProfilePhoto
        [HttpPost("ChangeProfilePicture-FreeLancer")]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> ChangeProfilePicture([FromForm] ChangeProfilePictureModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var userIdClaim = User.FindFirst("uid");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            // Update the profile picture URL
            if (file != null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string finalPath = assembly.GetName().Name;
                int index = wwwRootPath.IndexOf(finalPath); // Root path for web content
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Generate a unique file name
                string filePath = Path.Combine(index >= 0 ? wwwRootPath.Substring(index) : "", "FreeLancerProfileImage"); // Combine the path to the desired directory

                using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                model.NewProfilePictureUrl = fileName;
            }

            user.ProfilePicture = model.NewProfilePictureUrl;

            // Save the changes to the database
            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("Profile picture changed successfully");
            }
            else
            {
                return BadRequest("Failed to change profile picture");
            }
        }
        //Change-Name-Phone-Age-Language-ZIP-Address-Experience-Education-PortfolioURl-Description-YourTitle-HourlyRate
        [HttpPost("Change-Name-Phone-Age-Language-ZIP-Address-Experience-Education-PortfolioURl-Description-YourTitle-HourlyRate-Freelancer")]
        [Authorize(Roles = "Freelancer , Admin")]
        public async Task<IActionResult> ChangeFirstLastName([FromForm] ChangeNameModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var userIdClaim = User.FindFirst("uid");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Age = model.Age;
            user.ZIP = model.ZIP;
            user.Country = model.Country;
            user.State = model.State;
            user.Address = model.Address;
            user.Country = model.Country;
            user.State = model.State;
            user.Experience = model.Experience;
            user.Education = model.Education;
            user.PortfolioURl = model.PortfolioURl;
            user.Description = model.Description;
            user.YourTitle = model.YourTitle;
            user.HourlyRate = model.HourlyRate;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("successfully");
            }
            else
            {
                return BadRequest("Failed to change");
            }
        }
        //country
        [HttpPost("Change-Country-For-User")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> changecountry([FromForm] ChangeCountyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model");
            }

            var useridclaim = User.FindFirst("uid");

            if (useridclaim == null || string.IsNullOrEmpty(useridclaim.Value))
            {
                return BadRequest("user id not found in claims");
            }

            var user = await _userManager.FindByIdAsync(useridclaim.Value);

            // update the profile picture url
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Country = model.Country;

            // save the changes to the database
            var updateresult = await _userManager.UpdateAsync(user);

            if (updateresult.Succeeded)
            {
                return Ok("your country changed successfully");
            }
            else
            {
                return BadRequest("failed to change your country");
            }
        }
        [HttpPost("Change-Skills-Only")]
        [Authorize(Roles = "Freelancer, Admin")]
        public async Task<IActionResult> ChangeSkillsOnly([FromForm] ChangeSkillOnly model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var userIdClaim = User.FindFirst("uid");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.Users
                .Include(u => u.UserSkills)
                .FirstOrDefaultAsync(u => u.Id == userIdClaim.Value);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // الحصول على قائمة المهارات الحالية
            var existingSkills = user.UserSkills.Select(us => us.SkillId).ToList();

            // التحقق من المهارات الجديدة
            var newSkills = model.SelectedSkills.Where(skillId => !existingSkills.Contains(skillId)).ToList();

            if (newSkills.Count == 0)
            {
                return BadRequest("Cannot add duplicate skills");
            }

            // التحقق من أن جميع المهارات الجديدة موجودة في قاعدة البيانات
            var allSkillsInDb = _unitOfWork.Skill.GetAll();

            var invalidSkills = newSkills.Where(skillId => !allSkillsInDb.Any(s => s.Id == skillId)).ToList();

            if (invalidSkills.Any())
            {
                return BadRequest($"The following skills are not valid: {string.Join(", ", invalidSkills)}");
            }

            // إضافة المهارات الجديدة
            user.UserSkills.AddRange(newSkills.Select(skillId => new UserSkill { SkillId = skillId }));

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("Your skills changed successfully");
            }
            else
            {
                return BadRequest("Failed to change your skills");
            }
        }

        [HttpPost("Change-Languages-Only")]
        [Authorize(Roles = "Freelancer, Admin")]
        public async Task<IActionResult> ChangeLanguagesOnly([FromForm] ChangeLanguageOnly model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var userIdClaim = User.FindFirst("uid");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.Users
                .Include(u => u.UserLanguages)
                .FirstOrDefaultAsync(u => u.Id == userIdClaim.Value);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // الحصول على قائمة اللغات الحالية
            var existingLanguages = user.UserLanguages.Select(ul => ul.LanguageValue).ToList();

            // التحقق من اللغات الجديدة
            var newLanguages = model.SelectedLanguages.Where(language => !existingLanguages.Contains(language)).ToList();

            if (newLanguages.Count == 0)
            {
                return BadRequest("Cannot add duplicate languages");
            }

            // التحقق من أن جميع اللغات الجديدة موجودة في قاعدة البيانات
            var allLanguagesInDb = _unitOfWork.language.GetAll();

            var invalidLanguages = newLanguages.Where(language => !allLanguagesInDb.Any(l => l.Value == language)).ToList();

            if (invalidLanguages.Any())
            {
                return BadRequest($"The following languages are not valid: {string.Join(", ", invalidLanguages)}");
            }

            // إضافة اللغات الجديدة
            user.UserLanguages.AddRange(newLanguages.Select(language => new ApplicationUserLanguage { LanguageValue = language }));

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("Your languages changed successfully");
            }
            else
            {
                return BadRequest("Failed to change your languages");
            }
        }

        //Shills
        [HttpPost("ChangeSkilles-Freelancer")]
        [Authorize(Roles = "Freelancer , Admin")]
        public async Task<IActionResult> ChangeSkilles([FromForm] ChangeSkillesModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }

            var userIdClaim = User.FindFirst("uid");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            user.UserSkills = model.SelectedSkills
                .Select(skillId => new UserSkill { SkillId = skillId })
                .ToList();
            user.UserLanguages = model.SelectedLanguages
                .Select(language => new ApplicationUserLanguage { LanguageValue = language })
                .ToList();

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok("Your skills changed successfully");
            }
            else
            {
                return BadRequest("Failed to change your skills");
            }
        }

        [HttpGet("Get-User-Skills")]
        [Authorize(Roles = "Freelancer, Admin")]
        public async Task<IActionResult> GetUserSkills()
        {
            string userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.Users
                .Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userSkills = user.UserSkills.Select(us => new { us.Skill.Id, us.Skill.Name }).ToList();

            if (userSkills.Any())
            {
                return Ok(userSkills);
            }

            return NotFound("No skills found for this user");
        }

        [HttpGet("Get-User-Languages")]
        [Authorize]
        public async Task<IActionResult> GetUserLanguages()
        {
            string userId = User.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in claims");
            }

            var user = await _userManager.Users
                .Include(u => u.UserLanguages)
                .ThenInclude(ul => ul.Language)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userLanguages = user.UserLanguages.Select(ul => new { ul.Language.Id, ul.Language.Value }).ToList();

            if (userLanguages.Any())
            {
                return Ok(userLanguages);
            }

            return NotFound("No languages found for this user");
        }

        //AboutYou  Experience  Education  PortfolioURl  Description  YourTitle  HourlyRate
        //[HttpPost("ChangeAboutYou")]
        //[Authorize(Roles = "Freelancer , Admin")]
        //public async Task<IActionResult> ChangeAboutYou([FromBody] ChangeAboutModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid model");
        //    }

        //    var userIdClaim = User.FindFirst("uid");

        //    if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        //    {
        //        return BadRequest("User ID not found in claims");
        //    }

        //    var user = await _userManager.FindByIdAsync(userIdClaim.Value);

        //    user.Experience = model.Experience;
        //    user.Education = model.Education;
        //    user.PortfolioURl = model.PortfolioURl;
        //    user.Description = model.Description;
        //    user.YourTitle = model.YourTitle;
        //    user.HourlyRate = model.HourlyRate;


        //    // Save the changes to the database
        //    var updateResult = await _userManager.UpdateAsync(user);

        //    if (updateResult.Succeeded)
        //    {
        //        return Ok("Your Ditalis changed successfully");
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to change your Ditalis");
        //    }
        //}

        ////PhoneNumber
        //[HttpPost("ChangePhoneNumber")]
        //public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumber model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid model");
        //    }

        //    var userIdClaim = User.FindFirst("uid");

        //    if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        //    {
        //        return BadRequest("User ID not found in claims");
        //    }

        //    var user = await _userManager.FindByIdAsync(userIdClaim.Value);

        //    user.PhoneNumber = model.PhoneNumber;
        //    //user.CodePhone = model.CodePhone;

        //    var updateResult = await _userManager.UpdateAsync(user);

        //    if (updateResult.Succeeded)
        //    {
        //        return Ok("Your PhoneNumber changed successfully");
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to change your PhoneNumber");
        //    }
        //}

        ////Age  Language   ZIP  Address
        //[Authorize(Roles = "Freelancer , Admin")]
        //[HttpPost("ChangeYouDitalis")]
        //public async Task<IActionResult> ChangeYouDitalis([FromBody] ChangeYouDitalisModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid model");
        //    }

        //    var userIdClaim = User.FindFirst("uid");

        //    if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        //    {
        //        return BadRequest("User ID not found in claims");
        //    }

        //    var user = await _userManager.FindByIdAsync(userIdClaim.Value);

        //    user.Age = model.Age;
        //    user.ZIP = model.ZIP;
        //    user.Address = model.Address;
        //    user.UserLanguages = model.SelectedLanguages
        //        .Select(language => new ApplicationUserLanguage { LanguageValue = language })
        //        .ToList();
        //    // Save the changes to the database
        //    var updateResult = await _userManager.UpdateAsync(user);

        //    if (updateResult.Succeeded)
        //    {
        //        return Ok("Your Ditalis changed successfully");
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to change your Ditalis");
        //    }
        //}

        //country



    }
}


