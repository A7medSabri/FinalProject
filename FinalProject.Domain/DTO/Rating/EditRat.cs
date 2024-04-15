using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.Rating
{
    public class EditRat
    {
        [Range(1, 5)]
        public int Rate { get; set; }

        [Range(0, 100)]
        public int TaskCompletesPersentage { get; set; }
        [MaxLength(1000)]
        public string? Comments { get; set; }
    }
}
