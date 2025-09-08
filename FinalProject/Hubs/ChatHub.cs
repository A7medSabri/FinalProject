using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models;
using FinalProject.Domain.Models.ApplicationUserModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        //public ChatHub(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        //{
        //    _unitOfWork = unitOfWork;
        //    _userManager = userManager;
        //}
        //public async Task JoinToChatRoom(ApplicationUser user)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, user.Id);
        //    await Clients.Group(user.Id)
        //        .SendAsync("ReciveMessage", "admin", $"{user.FirstName + " " + user.LastName} has joined to the room");
        //}
        public async Task SendMessage(string userId, string receiverId, string message)
        {
            // Broadcast the message to all clients or specific clients
            await Clients.User(receiverId).SendAsync("ReceiveMessage", userId, message);
        }

        public override async Task OnConnectedAsync()
        {
            // Code to handle a new connection, if needed
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Code to handle a disconnection, if needed
            await base.OnDisconnectedAsync(exception);
        }


    }
}
