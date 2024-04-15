using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.JobPostAndContract;

namespace FinalProject.Domain.Models.FavoritesTable
{
    public class FavoritesFreelancer
    {
        [Key]
        public int Id { get; set; }
        //User
        public string ClientId { get; set; }

        [ForeignKey("Freelancer")]
        public string FreelancerId { get; set; }
        public ApplicationUser Freelancer { get; set; }

        //////Jobpost
        //[ForeignKey("Jobpost")]
        //public int? JobpostId { get; set; }
        //public JobPost Jobpost { get; set; }

    }
}
