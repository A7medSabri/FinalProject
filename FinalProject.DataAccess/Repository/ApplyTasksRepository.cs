using FinalProject.DataAccess.Data;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class ApplyTasksRepository : Repository<ApplyTask>, IApplyTasksRepository
    {
        ApplicationDbContext _context;

        public ApplyTasksRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
