using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.ApplyTasks
{
    public class offerApply
    {
        public int jobId { get; set; }
        public decimal price { get; set; }
        public string? offerDescription{ get; set; }
    }
}
