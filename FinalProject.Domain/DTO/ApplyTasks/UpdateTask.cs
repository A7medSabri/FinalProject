using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.NotificationAndMessageModel;
using FinalProject.Domain.Models.RatingModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class UpdateTask
    {
        public DateTime? DeliveryDate { get; set; }

        [RegularExpression("^(Pending|Complete|In progress|Cancel)$", ErrorMessage = "Status must be either 'Pending' or 'Complete' or 'In progress' or 'Cancel'.")]
        public string? Status { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }

        
    }
}
