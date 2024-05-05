using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Models.FavoritesTable
{
    public class GetAllFavFree
    {
       // public string Client { get; set; }
        //public string Freelancer { get; set; }
        public string FreelancerID { get; set; }
        public string FullName { get; set; }
        public string YourTitle { get; set; }
        public string Description { get; set; }
        public string ProfilePicture { get; set; }
        public decimal HourlyRate { get; set; }
        public bool IsFav { get; set; }
        public int Rate { get; set; }
    }
}
