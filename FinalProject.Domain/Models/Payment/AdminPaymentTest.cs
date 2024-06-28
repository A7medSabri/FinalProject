using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Models.Payment
{
    public class AdminPaymentTest
    {

        public int price { get; set; }

        public DateTime PayTime { get; set; }


        // Freelancer
        [ForeignKey("Freelancer")]
        public string FreelancerId { get; set; }
        public string ClientId { get; set; }
        public string JopName { get; set; }
    }
}
