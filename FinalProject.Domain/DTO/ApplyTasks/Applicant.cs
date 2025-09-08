using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class Applicant
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? Title { get; set; }
        public string? ProfilePictureUrl { get; set;}
        public decimal? hourlyRate { get; set; }
        public bool? isFavourite { get; set; }
        // Other applicant properties
    }
}
