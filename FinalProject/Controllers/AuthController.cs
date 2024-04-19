﻿using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Identity.DtoUserAndFreelancerRegister;
using FinalProject.Identity.Login;
using FinalProject.Identity.Password;
using FinalProject.Identity.Role;
using FinalProject.Identity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using UserMangmentService.Service;
using static System.Net.WebRequestMethods;

namespace FinalProject.Controllers
{
    //[Route("/[controller]")]
    [Route("/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailServices _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(IAuthService authService, IEmailServices emailService, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _authService = authService;
            _emailService = emailService;
            _userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("Register-Freelance")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterFreelancerAsync([FromForm] RegisterFreelanceModel model, IFormFile file )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.RegisterFreelancerAsync(model,file , "Freelancer" );

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }


            var user = await _userManager.FindByEmailAsync(result.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
            var urlEncoder = UrlEncoder.Default;
            var encodedToken = urlEncoder.Encode(token);
            var confirmationLink = $"http://localhost:3000/congratulation?token={token}&email={user.Email}";
            //var confirmationLink = $"http://localhost:5238/Confirm-Email?token={encodedToken}&email={user.Email}";

            //host 3000
            var message = new UserMangmentService.Models.Message(new string[] { user.Email! }, "Email Confirmation", confirmationLink!);
            _emailService.SendEmail(message);

            result.IsAuthenticated = false;

            return Ok(result);

        }
        [HttpPost("Register-User")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync([FromForm] RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterUserAsync(model, "User");

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }


            var user = await _userManager.FindByEmailAsync(result.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);

            var urlEncoder = UrlEncoder.Default;
            var encodedToken = urlEncoder.Encode(token);
            var confirmationLink = $"http://localhost:3000/congratulation?token={token}&email={user.Email}";
            //var confirmationLink = $"http://localhost:5238/Confirm-Email?token={encodedToken}&email={user.Email}";

            //host 3000
            var message = new UserMangmentService.Models.Message(new string[] { user.Email! }, "Email Confirmation", confirmationLink!);
            _emailService.SendEmail(message);

            result.IsAuthenticated = false;

            return Ok(result);

        }

        [HttpPost("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok("Email Verified Successfully");
                }
                else
                {
                    return BadRequest("Invalid or expired token");
                }
            }

            return BadRequest("User not found");
        }


        [HttpPost("Resend-Confirmation-Link")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationLink([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return BadRequest("Email is already confirmed.");
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
                var urlEncoder = UrlEncoder.Default;
                var encodedToken = urlEncoder.Encode(token);
                var confirmationLink = $"http://localhost:3000/congratulation?token={token}&email={user.Email}";
                //var confirmationLink = $"http://localhost:5238/Confirm-Email?token={token}&email={user.Email}";

                var message = new UserMangmentService.Models.Message(new string[] { user.Email! }, "Email Confirmation", confirmationLink!);
                _emailService.SendEmail(message);

                //return Ok($"Link For Email Confirmation Sended To Your Email: {user.Email}");
                return Ok(token);
            }
            else
            {
                return BadRequest("Email not found. Please make sure the email is correct.");
            }
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return BadRequest("Email not confirmed");
                }
                var result = await _authService.GetTokenAsync(model);
                if (result.IsAuthenticated == false)
                {
                    return BadRequest(result.Message);
                }
                var roles = await _userManager.GetRolesAsync(user);
                string role = roles.FirstOrDefault() ?? "No role assigned";

                var response = new
                {
                    Token = result.Token,
                    Role = role
                };

                return Ok(response);
            }
            else
            {
                return BadRequest("Email not Sign Up");
            }
        }



        [HttpPost("Forgot-Password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var ForgotPasswordlink = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);

                var urlEncoder = UrlEncoder.Default;
                var encodedToken = urlEncoder.Encode(token);
                var ForgotPasswordlink = $"http://localhost:3000/resetPassword?token={token}&email={user.Email}";
                //var ForgotPasswordlink = $"http://localhost:5238/resetPassword?token={encodedToken}&email={user.Email}";

                var message = new UserMangmentService.Models.Message(new string[] { user.Email! }, "Reset Password", ForgotPasswordlink!);

                _emailService.SendEmail(message);
                var resetPasswordModel = new ResetPasswordModel { Token = token, Email = user.Email };

                return Ok(resetPasswordModel);
                //return Ok($"Link For Reset Password Sended To Your Email: {user.Email}");
            }
            else
            {
                return BadRequest("I Can't Find Email && Couldn't Send Email , Please Try Again");
            }
        }

        [HttpGet("resetPassword")]
        //[HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };

            return Ok(model);
        }

        [HttpPost]
        [Route("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var restPass = await _userManager.ResetPasswordAsync(user, model.Token, model.password);
                if (!restPass.Succeeded)
                {
                    foreach (var error in restPass.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Ok(ModelState);
                }
                return Ok("Password Changed");
            }
            else
            {
                return BadRequest("Please Try Again");
            }
        }

        [HttpPost("Resend-Reset-Password-Link")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendResetPasswordLink([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var resetPasswordLink = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);

                var urlEncoder = UrlEncoder.Default;
                var encodedToken = urlEncoder.Encode(token);
                var resetPasswordLink = $"http://localhost:3000/resetPassword?token={token}&email={user.Email}";
                //var resetPasswordLink = $"http://localhost:5238/resetPassword?token={encodedToken}&email={user.Email}";

                var message = new UserMangmentService.Models.Message(new string[] { user.Email! }, "Reset Password", resetPasswordLink!);
                _emailService.SendEmail(message);

                var resetPasswordModel = new ResetPasswordModel { Token = token, Email = user.Email };

                return Ok(resetPasswordModel);

                //return Ok($"Link For Reset Password Sended To Your Email: {user.Email}");
            }
            else
            {
                return BadRequest("Email not found. Please make sure the email is correct.");
            }
        }

        [HttpPost("SignOut")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();

                return Ok("Sign out successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Sign out: {ex.Message}");
            }
        }

        [HttpPost("Add-Role")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            return Ok(model);
        }



    }
}
