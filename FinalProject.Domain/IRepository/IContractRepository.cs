using FinalProject.Domain.DTO;
using FinalProject.Domain.DTO.Contract;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IContractRepository : IRepository<Contract>
    {

        Contract CreateNew(NewContractDto newContract ,string UserId);

        // Contract FindByJobPostId(int id);
        GetContract FindByJobPostId(int id);

        void Create (NewContractDto newContract, string UserId);
        Contract Update (NewContractDto contract);
        bool FindContract(Expression<Func<Contract, bool>> predicate);
        // List<NewContractDto> GetAll(string Id, string Role);
        List<GetContract> GetAll(string Id, string Role);

    }
}
