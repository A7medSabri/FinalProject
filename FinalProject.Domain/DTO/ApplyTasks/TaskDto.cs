using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class TaskDto
    {
       
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
        public int JobPostId { get; set; }
        public string ClientId { get; set; }
        public string FreelancerId { get; set; }

    }
}
