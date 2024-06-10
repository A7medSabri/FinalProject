using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class TaskDto
    {

        public int TaskId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public string? Taketitle { get; set; }
        public string? TakeDescription { get; set; }


        [RegularExpression("^(Pending|Complete|In progress|Cancel)$", ErrorMessage = "Status must be either 'Pending' or 'Complete' or 'In progress' or 'Cancel'.")]
        public string? Status { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }

        public int JobPostId { get; set; }

        public string? CategoryName { get; set; }
        public bool? isDeleted { get; set; }

    }
}
