using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Contract;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Contract CreateNew(NewContractDto newContract ,string UserId)
        {
            var NewContract = new Contract
            {
                TremsAndCondetions = newContract.TremsAndCondetions,
                ClientId = UserId,
                FreelancerId = newContract.FreelancerId,
                JopPostId = newContract.JopPostId,
                PaymentMethodId = newContract.PaymentMethodId ?? null,
                StartDate = newContract.StartDate,
                EndDate = newContract.EndDate,
                Price = newContract.Price,

            };
            _context.Contracts.Add(NewContract);
            return NewContract;
        }


        #region SaadAllah
        public void Create(NewContractDto contract, string UserId)
        {
            Contract con = new Contract();
            if (contract != null)
            {
                if (contract.Price != null && contract.Price > 0)
                {
                    con.Price = contract.Price;
                }
                if (contract.TremsAndCondetions != null)
                {
                    con.TremsAndCondetions = contract.TremsAndCondetions;
                }
                if (UserId != null)
                {
                    con.ClientId = UserId;
                }
                con.EndDate = contract.EndDate;
                con.StartDate = contract.StartDate;
                con.JopPostId = contract.JopPostId;
                if (contract.FreelancerId != null)
                    con.FreelancerId = contract.FreelancerId;
                con.PaymentMethodId = contract.PaymentMethodId;
                con.ContractDate = DateTime.Now;
                con.IsDeleted = false;
                _context.Contracts.Add(con);

            }
        }

        public bool FindContract(Expression<Func<Contract, bool>> predicate)
        {
            return _context.Set<Contract>().Any(predicate);
        }

        public Contract Update(NewContractDto contract)
        {
            Contract con = _context.Contracts.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.JopPostId == contract.JopPostId);
            if (con != null)
            {
                con.StartDate = contract.StartDate;
                con.EndDate = contract.EndDate;
                con.JopPostId = contract.JopPostId;
                if (contract.FreelancerId != null)
                    con.FreelancerId = contract.FreelancerId;
               
                if (contract.Price != null && contract.Price > 0)
                {
                    con.Price = contract.Price;
                }
                con.PaymentMethodId = contract.PaymentMethodId;
                if (contract.TremsAndCondetions != null)
                {
                    con.TremsAndCondetions = contract.TremsAndCondetions;
                }
                _context.Contracts.Update(con);
                return con;
            }
            return null;
        }

        public List<NewContractDto> GetAll(string Id, string Role)
        {
            if (Id != null && Role == "Freelancer")
            {
                var result = _context.Contracts.Where(c => c.FreelancerId == Id).ToList();
                NewContractDto NewContractDto = new NewContractDto();
                List<NewContractDto> contractDtolst = new List<NewContractDto>();
                if (result != null)
                {
                    foreach (var contract in result)
                    {
                        NewContractDto.StartDate = contract.StartDate;
                        NewContractDto.EndDate = contract.EndDate;
                        NewContractDto.TremsAndCondetions = contract.TremsAndCondetions;
                        NewContractDto.Price = contract.Price;
                        NewContractDto.FreelancerId = contract.FreelancerId;
                        NewContractDto.JopPostId = contract.JopPostId;
                        contractDtolst.Add(NewContractDto);
                    }

                }
                return contractDtolst;
                // return Ok(_unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList());
            }
            else if (Id != null && Role == "User")
            {
                var result = _context.Contracts.Where(c => c.ClientId == Id).ToList();
                NewContractDto NewContractDto = new NewContractDto();
                List<NewContractDto> contractDtolst = new List<NewContractDto>();
                if (result != null)
                {
                    foreach (var contract in result)
                    {
                        NewContractDto.StartDate = contract.StartDate;
                        NewContractDto.EndDate = contract.EndDate;
                        NewContractDto.TremsAndCondetions = contract.TremsAndCondetions;
                        NewContractDto.Price = contract.Price;
                        NewContractDto.FreelancerId = contract.FreelancerId;
                        NewContractDto.JopPostId = contract.JopPostId;
                        contractDtolst.Add(NewContractDto);
                    }

                }
                return contractDtolst;
                // return Ok(_unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList());
                //return Ok(_unitOfWork.Contract.Find(c => c.ClientId == userId).ToList());
            }
            else { return null; }
        }

        public Contract FindByJobPostId(int id)
        {
           Contract contract = _context.Contracts.FirstOrDefault(c => c.JopPostId == id && c.IsDeleted == false);
            return contract;
        }
        #endregion
    }
}
