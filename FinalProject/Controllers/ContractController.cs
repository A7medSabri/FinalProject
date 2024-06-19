using FinalProject.DataAccess.Repository;
using FinalProject.Domain.DTO;
using FinalProject.Domain.DTO.Contract;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.JobPostAndContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserMangmentService.Service;



namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContractController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailServices _emailService;

        public UserManager<ApplicationUser> _userManager { get; }

        public ContractController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager , IEmailServices emailServices)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailService = emailServices;
        }


        [HttpPost ("Create-New-Contract")]
        [Authorize(Roles = "User")]
        public IActionResult Post([FromForm] NewContractDto contract)
        {
            var userId = User.FindFirst("uid")?.Value;
            var result = _unitOfWork.Contract.FindByJobPostId(contract.JopPostId);
            if (result != null && result.IsDeleted == false)
                return BadRequest("This JobPost already has a Contract");
                    
                
            _unitOfWork.Contract.Create(contract, userId);
            if (ModelState.IsValid)
            {
                _unitOfWork.Save();

                var freelancer = _userManager.FindByIdAsync(contract.FreelancerId).Result;
                if (freelancer != null)
                {
                    //var confirmationLink = $"http://localhost:5238/api/Contract/findByJobPostId?id={contract.JopPostId}";
                    var confirmationLink = $"http://localhost:3000/ContractDetails/id={contract.JopPostId}";

                    var message = new UserMangmentService.Models.Message(new string[] { freelancer.Email! }, "New Contract Created", confirmationLink!);
                    _emailService.SendEmail(message);
                }

                return Ok(contract);
            }
            return BadRequest();
        }














        [HttpGet("findByJobPostId")]
        public IActionResult Get(int id)
        {
            Contract result = _unitOfWork.Contract.Find(c => c.JopPostId == id).Where(c => c.IsDeleted == false).FirstOrDefault();
            if (result == null)
            {
                return BadRequest("There is no Contract to this JobPost");
            }
            if (ModelState.IsValid)
            {
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            return NotFound("This contract doesn't exist");
        }

        [HttpGet("GetMyAllContracts")]
        public async Task<IActionResult> GetMyAllContracts()
        {
            var userId = User.FindFirst("uid")?.Value;
            ApplicationUser user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var userRole = await _userManager.GetRolesAsync(user);
            var Result = _unitOfWork.Contract.GetAll(userId, userRole[0]);
            if(Result != null)
            {
                return Ok(Result);
            }
            #region test
            //if (userId != null && userRole[0] == "Freelancer")
            //{
            //    var result = _unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList();
            //    NewContractDto NewContractDto = new NewContractDto();
            //    List<NewContractDto> contractDtolst = new List<NewContractDto>();
            //    if(result != null)
            //    {
            //        foreach(var contract in result)
            //        {
            //            NewContractDto.StartDate = contract.StartDate;
            //            NewContractDto.EndDate = contract.EndDate;
            //            NewContractDto.TremsAndCondetions = contract.TremsAndCondetions;
            //            NewContractDto.Price = contract.Price;
            //            NewContractDto.FreelancerId = contract.FreelancerId;
            //            NewContractDto.JopPostId = contract.JopPostId;
            //            contractDtolst.Add(NewContractDto);
            //        }

            //    }
            //    return Ok(NewContractDto);
            //  // return Ok(_unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList());
            //}
            //else if(userId != null && userRole[0] == "User")
            //{
            //    var result = _unitOfWork.Contract.Find(c => c.ClientId == userId).ToList();
            //    NewContractDto NewContractDto = new NewContractDto();
            //    List<NewContractDto> contractDtolst = new List<NewContractDto>();
            //    if (result != null)
            //    {
            //        foreach (var contract in result)
            //        {
            //            NewContractDto.StartDate = contract.StartDate;
            //            NewContractDto.EndDate = contract.EndDate;
            //            NewContractDto.TremsAndCondetions = contract.TremsAndCondetions;
            //            NewContractDto.Price = contract.Price;
            //            NewContractDto.FreelancerId = contract.FreelancerId;
            //            NewContractDto.JopPostId = contract.JopPostId;
            //            contractDtolst.Add(NewContractDto);
            //        }

            //    }
            //    return Ok(NewContractDto);
            //    // return Ok(_unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList());
            //    //return Ok(_unitOfWork.Contract.Find(c => c.ClientId == userId).ToList());
            //}
            #endregion
            else
            {
                return Unauthorized();
            }
                
        }

        //[HttpPost]
        //[Authorize(Roles = "User")]
        //public IActionResult Post([FromForm] ContractDTO contract)
        //{
        //    Contract con = new Contract();
        //    var result = _unitOfWork.Contract.Find(c => c.JopPostId == contract.JopPostId).ToList();
        //    if (result != null) 
        //    foreach (var i in result)
        //    {
        //        if (i != null && i.IsDeleted == false)
        //        {
        //            return BadRequest("This JobPost already has a Contract");
        //        }
        //    }


        //    con.StartDate = contract.StartDate;
        //    con.EndDate = contract.EndDate;
        //    con.FreelancerId = contract.FreelancerId;
        //    con.JopPostId = contract.JopPostId;
        //    con.ClientId = User.FindFirst("uid")?.Value;
        //    con.Price = contract.Price;
        //    con.TremsAndCondetions = contract.TremsAndCondetions;
        //    con.PaymentMethodId = contract.PaymentMethodId;
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Contract.Create(con);
        //        _unitOfWork.Save();
        //        return Ok(con);
        //    }
        //    return BadRequest();
        //}

        [HttpPut("UpdateContract")]
        [Authorize(Roles = "User")]
        public IActionResult Update([FromForm] NewContractDto contract) 
        {

            //Contract con = _unitOfWork.Contract.Find(c => c.JopPostId == contract.JopPostId).Where(c => c.IsDeleted == false).FirstOrDefault();
            //con.StartDate = contract.StartDate;
            //con.EndDate = contract.EndDate;
            //con.FreelancerId = contract.FreelancerId;
            //con.JopPostId = contract.JopPostId;
            //con.ClientId = contract.ClientId;
            //con.Price = contract.Price;
            //con.PaymentMethodId = contract.PaymentMethodId;
            //con.TremsAndCondetions = contract.TremsAndCondetions;
            if (ModelState.IsValid)
            {

                var result = _unitOfWork.Contract.Update(contract);
                if (result != null)
                {
                    _unitOfWork.Save();
                    return Ok(result);
                }
            }
            return BadRequest();
        }

        [HttpDelete("CancelContract")]
        public IActionResult Delete(int id)
        {
            Contract conract = _unitOfWork.Contract.GetByID(id);
            if (conract != null && conract.IsDeleted == false)
            {
                if (ModelState.IsValid)
                {
                    conract.IsDeleted = true;
                    _unitOfWork.Save();
                    return Ok(conract);
                }
            }
            return NotFound("This contract already deleted.");
        }

        [HttpPost("SubmitContract")]
        [Authorize(Roles = "Freelancer")]
        public IActionResult submitContract([FromForm] string email)
        {
            var id = User.FindFirst("uid")?.Value;
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user != null && user.Email == email)
            {
                return Ok("You have successfully submitted the Contract.");
            }
            else
            {
                return BadRequest("Please make sure that you enter a valid email address");
            }
        }

    }
}
