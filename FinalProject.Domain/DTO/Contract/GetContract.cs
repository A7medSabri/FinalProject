using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.Contract
{
    public class GetContract
    {
        [Required]
        public string TremsAndCondetions { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int JopPostId { get; set; }

        public string? jopPostName { get; set; }

        public string? jopPostDescription { get; set; }
        [Required]
        public string FreelancerId { get; set; }

        public string? FreelancerName { get; set; }
        public string? ClinetName { get; set; }

        public int? PaymentMethodId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
