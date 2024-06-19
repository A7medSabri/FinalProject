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
        public void Create(Contract contract)
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
                if (contract.ClientId != null)
                {
                    con.ClientId = contract.ClientId;
                }
                con.EndDate = contract.EndDate;
                con.StartDate = contract.StartDate;
                con.JopPostId = contract.JopPostId;
                if (contract.FreelancerId != null)
                    con.FreelancerId = contract.FreelancerId;
                con.PaymentMethodId = contract.PaymentMethodId;
                con.ContractDate = DateTime.Now;
                _context.Contracts.Add(con);

            }
        }

        public bool FindContract(Expression<Func<Contract, bool>> predicate)
        {
            return _context.Set<Contract>().Any(predicate);
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
        #endregion
    }
}
