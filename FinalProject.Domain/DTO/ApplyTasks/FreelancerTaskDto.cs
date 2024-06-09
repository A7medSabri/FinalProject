using FinalProject.Domain.Models.ApplicationUserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Domain.Models.SkillAndCat;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class FreelancerTaskDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        [RegularExpression("^(Pending|Complete|In progress|Cancel)$", ErrorMessage = "Status must be either 'Pending' or 'Complete' or 'In progress' or 'Cancel'.")]
        public string? Status { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }

        public int JobPostId { get; set; }

        public string ClientId { get; set; }

        public string ClientFullName { get; set; }
        public string ?title { get; set; }
        public string ?Description { get; set; }

        public string ?CategoryName { get; set; }
        public List<string> ?skills { get; set; }
        public bool ?isDeleted { get; set; }
        public bool ?isFav { get; set; }
       
    }
}
