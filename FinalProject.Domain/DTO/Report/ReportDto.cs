using FinalProject.Domain.Models.ApplicationUserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.Report
{
    public class ReportDto
    {
        [Required]
        public string Type { get; set; }

        [MaxLength(1000)]
        [MinLength(2)]
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        public string ClientId { get; set; }

        public string? FreeLanceUserName { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
