using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IContractRepository : IRepository<Contract>
    {
        void Create (Contract contract);
        void Update (Contract contract);
        
    }
}
