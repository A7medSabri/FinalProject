﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.JobPost
{
    public class GetMyJobPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? DurationTime { get; set; }
        public string CategoryName { get; set; }
        public List<string>? JobPostSkill { get; set; }
        public string? UserFullName { get; set; }
        public string Status { get; set; }

    }
}
