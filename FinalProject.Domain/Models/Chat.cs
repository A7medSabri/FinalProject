using FinalProject.Domain.Models.ApplicationUserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Models
{
    public class Chat
    {
        [Required]
        public string Message { get; set; }
        public DateTime DateAndTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        // FreeLancer
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        // Client
        [ForeignKey("Receiver")]
        public string ReceiverrId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}
