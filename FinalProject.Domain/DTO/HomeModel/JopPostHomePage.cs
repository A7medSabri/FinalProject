using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.DTO.HomeModel
{
    public class JopPostHomePage
    {
        public int id { get; set; }
        public string Title { get; set; }


        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }
    }
}
