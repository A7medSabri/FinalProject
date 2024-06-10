using AutoMapper;
using FinalProject.Domain.DTO.ApplyTasks;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile() 
        { 
          CreateMap<ApplyTask,TaskDto>().ReverseMap();
          CreateMap<UpdateTask, ApplyTask>().ReverseMap();

        }
    }
}
