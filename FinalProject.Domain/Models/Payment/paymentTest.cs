﻿using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.JobPostAndContract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Domain.Models.Payment
{
    public class paymentTest
    {
        [Key]
        public int Id { get; set; }

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
        public int price { get; set; }
        public DateTime PayTime { get; set; }
        // Client
        [ForeignKey("Client")]
        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; }

        // Freelancer
        [ForeignKey("Freelancer")]
        public string FreelancerId { get; set; }
        public ApplicationUser Freelancer { get; set; }

        [ForeignKey("jobPost")]
        public int jobId { get; set; }
        public JobPost jobPost { get; set; }
    }
}
