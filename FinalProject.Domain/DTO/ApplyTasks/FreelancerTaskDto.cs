using FinalProject.Domain.Models.ApplicationUserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Domain.Models.SkillAndCat;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class FreelancerTaskDto : TaskDto
    {

        public string ClientId { get; set; }

        public string ClientFullName { get; set; }
        public bool ?isFav { get; set; }
       
    }
}
