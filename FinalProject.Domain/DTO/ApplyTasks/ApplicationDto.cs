using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class ApplicationDto
    {
        [Required]
        public int JobPostId{  get; set; }
        [Required]
        public DateTime? DeliveryDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }

    }
}
