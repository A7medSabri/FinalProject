using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Here you can fetch user ID from connection claims or any other method
            return connection.User?.FindFirst("uid")?.Value;
        }
    }
}
