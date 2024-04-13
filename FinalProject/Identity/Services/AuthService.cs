using FinalProject.DataAccess.Data;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.RegisterNeeded;
using FinalProject.Domain.Models.SkillAndCat;
using FinalProject.Identity.Dto;
using FinalProject.Identity.Dto.Helper;
using FinalProject.Identity.DtoUserAndFreelancerRegister;
using FinalProject.Identity.Login;
using FinalProject.Identity.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProject.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt,
            ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<AuthModel> RegisterFreelancerAsync(RegisterFreelanceModel model, IFormFile? file, string selectedRole = "Freelancer")
        {
            var authModel = new AuthModel();
            List<ApplicationUserLanguage> UserLanguages = new List<ApplicationUserLanguage>();
            List<UserSkill> userSkills = new List<UserSkill>();
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                authModel.Message = "Email is already registered!";
                return authModel;
            }
            if (file != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // Root path for web content
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Generate a unique file name
                string filePath = Path.Combine(wwwRootPath, "FreeLancerProfileImage"); // Combine the path to the desired directory

                try
                {
                    using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream); 
                    }

                    model.ProfilePicture = fileName;
                }
                catch (Exception ex)
                {
                    authModel.Message = $"Error uploading profile picture: {ex.Message}";
                    return authModel;
                }
            }
            else
            {
                authModel.Message = "Profile picture is required.";
                return authModel;
            }

            if (model.SelectedLanguages != null)
            {
                foreach (var langValue in model.SelectedLanguages)
                {
                    var language = await _context.Languages.FindAsync(langValue);
                    if (language == null)
                    {
                        authModel.Message = $"Invalid Language ID: {langValue}.";
                        return authModel;
                    }

                    UserLanguages.Add(new ApplicationUserLanguage
                    {
                        LanguageValue = langValue,
                    });
                }
            }
            else
            {
                authModel.Message = "Please enter your languages.";
                return authModel;
            }

            if (model.SelectedSkills != null)
            {
                foreach (var skillValue in model.SelectedSkills)
                {
                    var skill = await _context.Skills.FindAsync(skillValue);
                    if (skill == null)
                    {
                        authModel.Message = $"Invalid Skill ID: {skillValue}.";
                        return authModel;
                    }

                    userSkills.Add(new UserSkill
                    {
                        SkillId = skillValue,
                    });
                }
            }
            else
            {
                authModel.Message = "Please enter your skills.";
                return authModel;
            }

            if (model.Address == null)
            {
                authModel.Message = $"Address Required";
                return authModel;
            }

            if (model.PhoneNumber == null)
            {
                authModel.Message = $"PhoneNumber Required";
                return authModel;
            }
            if (model.Age == null)
            {
                authModel.Message = $"Age Required";
                return authModel;
            }
            if (model.Description == null)
            {
                authModel.Message = $"Description Required";
                return authModel;
            }

            if (model.YourTitle == null)
            {
                authModel.Message = $"YourTitle Required";
                return authModel;
            }
            if (model.ZIP == null)
            {
                authModel.Message = $"ZIP Required";
                return authModel;
            }
            if (model.PortfolioURl == null)
            {
                authModel.Message = $"Portfolio URl Required";
                return authModel;
            }
            var user = new ApplicationUser
            {
                UserName = UserNameGenrator(model.FirstName, model.LastName),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Country = model.Country,
                State = model.State,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                UserLanguages = UserLanguages,
                PortfolioURl = model.PortfolioURl,
                ProfilePicture = model.ProfilePicture,
                Description = model.Description,
                UserSkills = userSkills,
                ZIP = model.ZIP,
                YourTitle = model.YourTitle,
                Education = model.Education,
                HourlyRate = model.HourlyRate,
                Experience = model.Experience,
                RegistrationDate = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {

                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                authModel.Message = $"User registration failed: {errors}";
                return authModel;
            }

            var userId = user.Id;

            foreach (var userLanguage in UserLanguages)
            {
                userLanguage.ApplicationUserId = userId;
            }

            foreach (var userSkill in userSkills)
            {
                userSkill.UserId = userId;
            }

            //_context.ApplicationUserLanguages.AddRange(UserLanguages);
            //_context.UserSkills.AddRange(userSkills);

            await _context.SaveChangesAsync();

            await _userManager.AddToRoleAsync(user, "Freelancer");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                Country = user.Country,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = (List<string>)await _userManager.GetRolesAsync(user),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = UserNameGenrator(model.FirstName, model.LastName)
            };

        }

        //public async Task<AuthModel> RegisterFreelancerAsync(RegisterFreelanceModel model, string selectedRole = "Freelancer" )
        //{
        //    var authModel = new AuthModel();

        //    if (await _userManager.FindByEmailAsync(model.Email) != null)
        //    {
        //        authModel.Message = "Email is already registered!";
        //        return authModel;
        //    }

        //    if (model.ProfilePicture != null)
        //    {
        //        //Write Your Code Here
        //    }
        //    else
        //    {
        //        authModel.Message = "Profile picture is required.";
        //        return authModel;
        //    }

        //    if (model.SelectedLanguages != null)
        //        {
        //            foreach (var langValue in model.SelectedLanguages)
        //            {
        //                var language = _context.Languages.FirstOrDefault(l => l.Id == langValue);

        //                if (language == null)
        //                {
        //                    authModel.Message = $"Invalid Languages : {langValue}.";
        //                    return authModel;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            authModel.Message = "Enter Your Languages.";
        //            return authModel;
        //        }


        //        var UserLanguages = model.SelectedLanguages?.Select(langValue => new ApplicationUserLanguage
        //        {
        //            LanguageValue = langValue,
        //        }).ToList();
        //        if (UserLanguages == null)
        //        {
        //            authModel.Message = "Please Enter Your Languges";
        //            return authModel;
        //        }

        //        //Skill
        //        var userSkills = model.SelectedSkills?.Select(skillName => new UserSkill
        //        {
        //            SkillId = skillName,
        //        }).ToList();
        //        if (userSkills == null || !userSkills.Any())
        //        {
        //            authModel.Message = "Please Enter Your Skills";
        //            return authModel;
        //        }

        //        if (model.Address == null)
        //        {
        //            authModel.Message = $"Address Required";
        //            return authModel;
        //        }
        //        //if (model.Phonecode == null)
        //        //{
        //        //    authModel.Message = $"Phonecode Required";
        //        //    return authModel;
        //        //}
        //        if (model.PhoneNumber == null)
        //        {
        //            authModel.Message = $"PhoneNumber Required";
        //            return authModel;
        //        }
        //        if (model.Age == null)
        //        {
        //            authModel.Message = $"Age Required";
        //            return authModel;
        //        }
        //        if (model.Description == null)
        //        {
        //            authModel.Message = $"Description Required";
        //            return authModel;
        //        }

        //        if (model.YourTitle == null)
        //        {
        //            authModel.Message = $"YourTitle Required";
        //            return authModel;
        //        }
        //        if (model.ZIP == null)
        //        {
        //            authModel.Message = $"ZIP Required";
        //            return authModel;
        //        }
        //        if (model.PortfolioURl == null)
        //        {
        //            authModel.Message = $"Portfolio URl Required";
        //            return authModel;
        //        }
        //        var user = new ApplicationUser
        //        {
        //            UserName = UserNameGenrator(model.FirstName,model.LastName),
        //            Email = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            Country = model.Country ,
        //            State = model.State,
        //            Address = model.Address,
        //            //CodePhone = model.Phonecode,
        //            PhoneNumber = model.PhoneNumber,
        //            Age = model.Age,
        //            UserLanguages = UserLanguages,
        //            PortfolioURl = model.PortfolioURl,
        //            Description = model.Description,
        //            UserSkills = userSkills,
        //            ZIP = model.ZIP,
        //            YourTitle = model.YourTitle,
        //            Education = model.Education,
        //            HourlyRate = model.HourlyRate,
        //            Experience = model.Experience,
        //            RegistrationDate = DateTime.Now,
        //        };

        //        var result = await _userManager.CreateAsync(user, model.Password);

        //        if (!result.Succeeded)
        //        {
        //            var errors = string.Join(", ", result.Errors.Select(error => error.Description));
        //            authModel.Message = $"User registration failed: {errors}";
        //            return authModel;
        //        }

        //        await _userManager.AddToRoleAsync(user, "Freelancer");

        //        var jwtSecurityToken = await CreateJwtToken(user);

        //        return new AuthModel
        //        {
        //            Email = user.Email,
        //            Country = user.Country,
        //            ExpiresOn = jwtSecurityToken.ValidTo,
        //            IsAuthenticated = true,
        //            Roles = (List<string>)await _userManager.GetRolesAsync(user),
        //            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
        //            Username = UserNameGenrator(model.FirstName, model.LastName)
        //        };

        //}
        public async Task<AuthModel> RegisterUserAsync(RegisterUserModel model, string selectedRole = "User")
        {
            var authModel = new AuthModel();

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                authModel.Message = "Email is already registered!";
                return authModel;
            }

            //Country
            //if (model.Country == null)
            //{
            //    authModel.Message = "Enter Your Counrty Name.";
            //    return authModel;
            //}
            //var country = _context.Countries.FirstOrDefault(c => c.Id == model.Country);
            //if (country == null)
            //{
            //    authModel.Message = "Invalid Country Name.";
            //    return authModel;
            //}

            var user = new ApplicationUser
            {
                UserName = UserNameGenrator(model.FirstName, model.LastName),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Country = model.Country,
                RegistrationDate = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                authModel.Message = $"User registration failed: {errors}";
                return authModel;
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                Country = user.Country,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = (List<string>)await _userManager.GetRolesAsync(user),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = UserNameGenrator(model.FirstName, model.LastName),
            };

        }
        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var rolesList = await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;

            authModel.Roles = rolesList.ToList();

            return authModel;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }
        static string UserNameGenrator(string fname, string lname)
        {

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();

            string result = fname[0].ToString().ToUpper() + lname[0].ToString().ToUpper() + "_" + day + month + year[2] + year[3] + hour + minute + second;
            return result;
        }

    }
}
