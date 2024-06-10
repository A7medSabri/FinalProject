using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class ClientTaskDto : TaskDto
    {
        public string FreelancerId { get; set; }
        public string? FreelancerFullName { get; set; }
        public string? Freelancertitle { get; set; }
        public string? FreelancerDescription { get; set; }
        public string? FreelancerProfilePictureUrl { get; set; }
        public decimal? FreelancerhourlyRate { get; set; }
        public bool? isFavourite { get; set; }
        // Other  Freelancer properties
    }
}
