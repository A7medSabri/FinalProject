using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.Models.FavoritesTable
{
    public class MyFavJobPost
    {

        public int? jobPostId { get; set; }
        public string jobPostTiilte { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? DurationTime { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public bool IsFav { get; set; }
        public bool IsApplied { get; set; }
        public int? TaskId { get; set; }

    }

}
