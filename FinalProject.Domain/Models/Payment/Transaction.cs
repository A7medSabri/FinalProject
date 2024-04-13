using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Models.Payment
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression("^(pending|completed|failed)$")]
        public string PaymentStatus { get; set; }
        [Required]
        public decimal PaymentFee { get; set; }
        [Required]
        public decimal PlatformFee { get; set; }
        // DefaultValue now
        public DateTime TransactionTime { get; set; }

        // JobPost
        [ForeignKey("JobPost")]
        public int JobPostId { get; set; }
        public JobPost JobPost { get; set; }

        // Payment Methods
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        // User
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }  
        public ApplicationUser ApplicationUser { get; set; }

    }
}
