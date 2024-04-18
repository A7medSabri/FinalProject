using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Models.FavoritesTable
{
    public class FavJobPost
    {
        [Key]
        public int Id { get; set; }
        //User
        [ForeignKey("FreelancerId")]
        public string FreelancerId { get; set; }
        public ApplicationUser Freelancer { get; set; }

        //Jobpost
        [ForeignKey("Jobpost")]
        public int? JobpostId { get; set; }
        public JobPost Jobpost { get; set; }
    }
}
