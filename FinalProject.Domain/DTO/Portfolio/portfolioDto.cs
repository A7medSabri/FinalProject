using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.Portfolio
{
    public class portfolioDto
    {
        [MaxLength(40)]
        [MinLength(2)]
        [Required]
        public string Name { get; set; }

        [MaxLength(4000)]
        [MinLength(2)]
        [Required]
        public string Description { get; set; }

        public string? Media { get; set; }

        public string? URL { get; set; }

        public DateTime? ProjectDate { get; set; }

        public string UserId { get; set; }
    }
}
