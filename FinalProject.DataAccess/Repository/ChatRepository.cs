using FinalProject.DataAccess.Data;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        ApplicationDbContext _context;
        public ChatRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Dictionary<DateTime, string> GetMessages(string senderId, string receiverId)
        {
            var messages = _context.Chats
                .Where(c => c.IsDeleted == false && (c.SenderId == senderId || c.ReceiverrId == senderId) && (c.ReceiverrId == receiverId || c.SenderId == receiverId))
                .OrderBy(c => c.DateAndTime)
                .Select(c => new {DateAndTime = c.DateAndTime,Message = c.Message})
                .ToList();
            if (messages.IsNullOrEmpty())
            {
                return null;
            }
            Dictionary<DateTime, string> messagesDictionary = new Dictionary<DateTime, string>();
            foreach (var message in messages) 
            {
                messagesDictionary.Add(message.DateAndTime, message.Message);
            }

            return messagesDictionary;
        }

       

        public void SendMessage(string senderId, string receiverId, string Message)
        {
            _context.Chats.Add(new Chat 
            {
                SenderId = senderId,
                ReceiverrId = receiverId,
                Message = senderId + ": " + Message,
                DateAndTime = DateTime.Now
            });
        }
        public Dictionary<DateTime, string> GetNewMessages(string senderId, string receiverId , DateTime date)
        {
            var lastMessages = _context.Chats
                .Where(c => c.IsDeleted == false && (c.SenderId == senderId || c.ReceiverrId == senderId) && (c.ReceiverrId == receiverId || c.SenderId == receiverId) && c.DateAndTime > date)
                .OrderBy(c => c.DateAndTime)
                .Select(c => new { DateAndTime = c.DateAndTime, Message = c.Message })
                .ToList();
            Dictionary<DateTime, string> newMessages = new Dictionary<DateTime, string>();
            foreach (var message in lastMessages)
            {
                newMessages.Add(message.DateAndTime, message.Message);
            }
            return newMessages;
        }
        //public void SendMessage(string freelancerId, string clientId, string role, string Message)
        //{
        //    if (role == "Freelancer")
        //    {
        //        _context.Chats.Add(new Chat
        //        {
        //            FreeLancerId = freelancerId,
        //            ClientId = clientId,
        //            Message = "F: " + Message,
        //            DateAndTime = DateTime.Now,
        //        });

        //    }
        //    else if(role == "User")
        //    {
        //        _context.Chats.Add(new Chat
        //        {
        //            FreeLancerId = freelancerId,
        //            ClientId = clientId,
        //            Message = "C: " + Message,
        //            DateAndTime = DateTime.Now,
        //        });

        //    }
        //}
    }
}
