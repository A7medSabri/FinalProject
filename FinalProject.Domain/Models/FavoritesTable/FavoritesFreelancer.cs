using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.JobPostAndContract;

namespace FinalProject.Domain.Models.FavoritesTable
{
    public class FavoritesFreelancer
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public string ClientId { get; set; }
        public ApplicationUser? Client { get; set; }

        [ForeignKey("Freelancer")]
        public string FreelancerId { get; set; }

        [JsonIgnore]
        public ApplicationUser? Freelancer { get; set; }
    }
}
