﻿using FinalProject.DataAccess.Repository;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using UserMangmentService.Service;

namespace FinalProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly IEmailServices _emailService;


        public ChatController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHubContext<ChatHub> chatHubContext, IEmailServices emailServices)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _chatHubContext = chatHubContext;
            _emailService = emailServices;
        }
        [HttpGet("GetAllMyMessage")]
        public async Task<IActionResult> GetAllMyMessage(string id)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var id2 = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user == null || id2 == null)
            {
                return NotFound("User not founded.");
            }
            var messages = _unitOfWork.Chat.GetMessages(id, userId);
            if (messages.IsNullOrEmpty())
            {
                return NotFound("There are no messages");
            }
            return Ok(messages);
        }


        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] string id, [FromForm] string message)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var id2 = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user == null || id2 == null)
            {
                return NotFound("User not founded.");
            }
            if (message != null)
            {
                if (userId == id)
                    return BadRequest("You Can't Send To YourSelf");

                _unitOfWork.Chat.SendMessage(userId, id, message);
                _unitOfWork.Save();
                var UserEmail = _userManager.FindByIdAsync(id).Result;
                if (UserEmail != null)
                {
                    var confirmationLink = $"http://localhost:3000/";

                    var MessageToGmail = new UserMangmentService.Models.Message(new string[] { UserEmail.Email! }, "Check Inbox", confirmationLink!);
                    _emailService.SendChatEmail(MessageToGmail);
                }
                var chatHub = (IHubContext<ChatHub>)HttpContext.RequestServices.GetService(typeof(IHubContext<ChatHub>));
                await chatHub.Clients.All.SendAsync("ReceiveMessage", userId, message);

                //  await _chatHubContext.Clients.User(id).SendAsync("ReceiveMessage", userId, message);
            }
            return Ok(_unitOfWork.Chat.GetMessages(id, userId));

        }

        [HttpGet("GetNewMessages")]
        public async Task<IActionResult> GetNewMessages(string id, DateTime date)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null)
                return Unauthorized();
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var user2 = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user == null || user2 == null)
            {
                return NotFound();
            }
            var newMessages = _unitOfWork.Chat.GetNewMessages(id, userId, date);
            if (newMessages == null) 
            { 
                return NotFound("There is no new messages");
            }
            return Ok(newMessages);
        }


        //[HttpGet("GetAllMyMessage")]
        //[Authorize(Roles = "User , Freelancer")]
        //public async Task<IActionResult> GetAllMyMessage(string Id)
        //{
        //    var userId = User.FindFirst("uid")?.Value;
        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }
        //    var user = _userManager.Users
        //        .FirstOrDefault(u => u.Id == userId);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }
        //    var userRoles =await _userManager.GetRolesAsync(user);
        //    if (userRoles == null || userRoles.Count == 0)
        //    {
        //        return Unauthorized();
        //    }
        //    List<string> result = new List<string>();
        //    if (userRoles[0] == "Freelancer")
        //    {
        //        result= _unitOfWork.Chat.GetMessages(userId, Id);
        //        if (result == null)
        //        {
        //            return NotFound("There is no messages!");
        //        }
        //    }
        //    else if (userRoles[0] == "User")
        //    {
        //        result = _unitOfWork.Chat.GetMessages(Id, userId);
        //        if (result == null)
        //        {
        //            return NotFound("There is no messages!");
        //        }
        //    }

        //    return Ok(result);

        //}
        //[HttpPost("SendMessage")]
        //[Authorize(Roles = "User , Freelancer")]
        //public async Task<IActionResult> SendMessage(string Id, string message)
        //{
        //    var userId = User.FindFirst("uid")?.Value;
        //    if(userId == null)
        //    {
        //        return Unauthorized();
        //    }
        //    var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        //    if(user == null)
        //    {
        //        return NotFound("User not found");
        //    }
        //    var userRole = await _userManager.GetRolesAsync(user);
        //    if (userRole == null || userRole.Count == 0)
        //    {
        //        return Unauthorized();
        //    }
        //    if (userRole[0] == "Freelancer")
        //    {
        //        _unitOfWork.Chat.SendMessage(userId, Id, userRole[0], message);
        //        _unitOfWork.Save();
        //        return Ok(_unitOfWork.Chat.GetMessages(userId, Id));
        //    }
        //    else if (userRole[0] == "User")
        //    {
        //        _unitOfWork.Chat.SendMessage(Id, userId, userRole[0], message);
        //        _unitOfWork.Save();
        //        return Ok(_unitOfWork.Chat.GetMessages(Id, userId));
        //    }
        //    else
        //        return BadRequest();
        //}



    }
}