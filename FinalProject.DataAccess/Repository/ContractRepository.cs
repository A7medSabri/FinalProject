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
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        private readonly ApplicationDbContext _context;

        public ContractRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Create(Contract contract)
        {
            Contract con = new Contract();
            if(contract != null)
            {
                if(contract.Price != null && contract.Price > 0)
                {
                    con.Price = contract.Price;
                }
                if(contract.TremsAndCondetions != null)
                {
                    con.TremsAndCondetions = contract.TremsAndCondetions;
                }
                if(contract.ClientId != null)
                {
                    con.ClientId = contract.ClientId;
                }
                con.EndDate = contract.EndDate;
                con.StartDate = contract.StartDate;
                con.JopPostId = contract.JopPostId;
                if(contract.FreelancerId != null)
                    con.FreelancerId = contract.FreelancerId;
                con.PaymentMethodId = contract.PaymentMethodId;
                con.ContractDate = DateTime.Now;
                _context.Contracts.Add(con);
                
            }
        }

        public void Update(Contract contract)
        {
            Contract con = _context.Contracts.FirstOrDefault(c => c.Id == contract.Id);
            if (con != null)
            {
                if (contract.Price != null && contract.Price > 0)
                {
                    con.Price = contract.Price;
                }
                if (contract.TremsAndCondetions != null)
                {
                    con.TremsAndCondetions = contract.TremsAndCondetions;
                }
                if (contract.ClientId != null)
                {
                    con.ClientId = contract.ClientId;
                }
                if (contract.FreelancerId != null)
                    con.FreelancerId = contract.FreelancerId;
                con.EndDate = contract.EndDate;
                con.StartDate = contract.StartDate;
                con.JopPostId = contract.JopPostId;
                con.PaymentMethodId = contract.PaymentMethodId;
                con.ContractDate = DateTime.Now;
                _context.Contracts.Update(con);
               
            }
            
        }
    }
}
