using FinalProject.Domain.Models.ApplicationUserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Domain.Models.Payment
{
    public class PaymentTestDto
    {
        public string Owner { get; set; }

        [MinLength(12)]
        [MaxLength(18)]
        public string CardNumber { get; set; }

        [Range(1, 12)]
        public int MM { get; set; }

        [Range(24, 30)]
        public int YY { get; set; }

        [MinLength(3)]
        [MaxLength(3)]
        public string CVV { get; set; }

        // Freelancer
        [ForeignKey("Freelancer")]
        public string FreelancerId { get; set; }
    }
}
