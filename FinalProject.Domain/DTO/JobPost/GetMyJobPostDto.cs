using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.JobPost
{
    public class GetMyJobPostDto : GetJobPostDto
    {
        public string UserId { get; set; }
        public string? UserFullName { get; set; }

    }
}
