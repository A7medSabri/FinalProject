﻿using AutoMapper.Configuration.Annotations;
using FinalProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinalProject.Domain.IRepository
{
    public interface IChatRepository : IRepository<Chat>
    {
        Dictionary<DateTime, string> GetMessages(string senderId, string receiverId);
        void SendMessage(string senderId, string receiverId, string Message);
        Dictionary<DateTime, string> GetNewMessages(string senderId, string receiverId, DateTime date);
    }
}
