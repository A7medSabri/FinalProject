using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.Contract
{
    public class NewContractDto
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
        //[Required]
        //public string ClientId { get; set; }
        [Required]
        public string FreelancerId { get; set; }

        public int? PaymentMethodId { get; set; }

    }
}
