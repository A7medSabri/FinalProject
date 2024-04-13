using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO
{
    public class ContractDTO
    {
        [Required]
        public string TremsAndCondetions { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string PaymentWay { get; set; }
        public int JopPostId { get; set; }
        public string ClientId { get; set; }
        public string FreelancerId { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
