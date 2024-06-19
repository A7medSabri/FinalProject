using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Contract;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using System;
using System.Collections.Generic;
using System.Data;
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

        //public List<NewContractDto> GetAll(string Id, string Role)
        //{
        //    if (Id != null && Role == "Freelancer")
        //    {
        //        var result = _context.Contracts.Where(c => c.FreelancerId == Id && c.IsDeleted == false).ToList();
        //        NewContractDto NewContractDto = new NewContractDto();
        //        List<NewContractDto> contractDtolst = new List<NewContractDto>();
        //        if (result != null)
        //        {
        //            foreach (var contract in result)
        //            {
        //                NewContractDto.StartDate = contract.StartDate;
        //                NewContractDto.EndDate = contract.EndDate;
        //                NewContractDto.TremsAndCondetions = contract.TremsAndCondetions;
        //                NewContractDto.Price = contract.Price;
        //                NewContractDto.FreelancerId = contract.FreelancerId;
        //                NewContractDto.JopPostId = contract.JopPostId;
        //                contractDtolst.Add(NewContractDto);
        //            }

        //        }
        //        return contractDtolst;
        //        // return Ok(_unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList());
        //    }
        //    else if (Id != null && Role == "User")
        //    {
        //        var result = _context.Contracts.Where(c => c.ClientId == Id && c.IsDeleted == false).ToList();
        //        NewContractDto NewContractDto = new NewContractDto();
        //        List<NewContractDto> contractDtolst = new List<NewContractDto>();
        //        if (result != null)
        //        {
        //            foreach (var contract in result)
        //            {
        //                NewContractDto.StartDate = contract.StartDate;
        //                NewContractDto.EndDate = contract.EndDate;
        //                NewContractDto.TremsAndCondetions = contract.TremsAndCondetions;
        //                NewContractDto.Price = contract.Price;
        //                NewContractDto.FreelancerId = contract.FreelancerId;
        //                NewContractDto.JopPostId = contract.JopPostId;
        //                contractDtolst.Add(NewContractDto);
        //            }

        //        }
        //        return contractDtolst;
        //        // return Ok(_unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList());
        //        //return Ok(_unitOfWork.Contract.Find(c => c.ClientId == userId).ToList());
        //    }
        //    else { return null; }
        //}
        public List<GetContract> GetAll(string Id, string Role)
        {
            if (Id == null || (Role != "Freelancer" && Role != "User"))
            {
                return null;
            }

            List<Contract> result = new List<Contract>();

            if (Role == "Freelancer")
            {
                result = _context.Contracts.Where(c => c.FreelancerId == Id && c.IsDeleted == false).ToList();
            }
            else if (Role == "User")
            {
                result = _context.Contracts.Where(c => c.ClientId == Id && c.IsDeleted == false).ToList();
            }

            var contractsWithJobPostDetails = result.Select(contract => {
                var jobPost = _context.JobPosts
                                .Where(j => j.Id == contract.JopPostId)
                                .Select(j => new { j.Title, j.Description })
                                .FirstOrDefault();

                var freelancer = _context.Users
                                    .Where(f => f.Id == (Role == "Freelancer" ? contract.ClientId : contract.FreelancerId))
                                    .Select(f => new { f.FirstName, f.LastName })
                                    .FirstOrDefault();
                var client = _context.Users
                                    .Where(f => f.Id == (Role == "User" ? contract.ClientId : contract.FreelancerId))
                                    .Select(f => new { f.FirstName, f.LastName })
                                    .FirstOrDefault();

                return new GetContract
                {
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    TremsAndCondetions = contract.TremsAndCondetions,
                    Price = contract.Price,
                    FreelancerId = contract.FreelancerId,
                    JopPostId = contract.JopPostId,
                    jopPostName = jobPost?.Title,
                    jopPostDescription = jobPost?.Description,
                    FreelancerName = freelancer.FirstName + " " + freelancer.LastName,
                    ClinetName = client.FirstName + " " + client.LastName,
                    IsDeleted = contract.IsDeleted,
                    contractID = contract.Id,
                };
            }).ToList();

            return contractsWithJobPostDetails;
        }

        public GetContract FindByJobPostId(int id)
        {
            GetContract contract = new GetContract();
            Contract con = _context.Contracts.FirstOrDefault(c => c.JopPostId == id && c.IsDeleted == false);

            if (con != null)
            {
                var jobPost = _context.JobPosts
                                .Where(j => j.Id == con.JopPostId)
                                .Select(j => new { j.Title, j.Description })
                                .FirstOrDefault();

                var freelancer = _context.Users
                                    .Find(con.FreelancerId);
                var clinet = _context.Users
                                    .Find(con.ClientId);
                contract.StartDate = con.StartDate;
                contract.EndDate = con.EndDate;
                contract.TremsAndCondetions = con.TremsAndCondetions;
                contract.Price = con.Price;
                contract.FreelancerId = con.FreelancerId;
                contract.JopPostId = con.JopPostId;
                contract.jopPostDescription = jobPost.Description;
                contract.FreelancerName  = freelancer.FirstName +" "+freelancer.LastName;
                contract.ClinetName  = clinet.FirstName +" "+clinet.LastName;
                contract.IsDeleted = con.IsDeleted;
                contract.contractID = con.Id;
            }
            return contract;
        }
        #endregion
    }
}
