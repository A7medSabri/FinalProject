using FinalProject.DataAccess.Repository;
using FinalProject.Domain.DTO;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ApplicationUserModel;
using FinalProject.Domain.Models.JobPostAndContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;



namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ContractController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager<ApplicationUser> _userManager { get; }

        public ContractController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
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
            if (userId != null && userRole[0] == "Freelancer")
            {
               return Ok(_unitOfWork.Contract.Find(c => c.FreelancerId == userId).ToList());
            }
            else if(userId != null && userRole[0] == "User")
            {
                return Ok(_unitOfWork.Contract.Find(c => c.ClientId == userId).ToList());
            }
            else
            {
                return Unauthorized();
            }
                
        }

        [HttpPost]
        public IActionResult Post([FromBody] ContractDTO contract)
        {
            Contract con = new Contract();
            var result = _unitOfWork.Contract.Find(c => c.JopPostId == contract.JopPostId).ToList();
            if (result != null) 
            foreach (var i in result)
            {
                if (i != null && i.IsDeleted == false)
                {
                    return BadRequest("This JobPost already has a Contract");
                }
            }


            con.StartDate = contract.StartDate;
            con.EndDate = contract.EndDate;
            con.FreelancerId = contract.FreelancerId;
            con.JopPostId = contract.JopPostId;
            con.ClientId = User.FindFirst("uid")?.Value;
            con.Price = contract.Price;
            con.TremsAndCondetions = contract.TremsAndCondetions;
            con.PaymentMethodId = contract.PaymentMethodId;
            if (ModelState.IsValid)
            {
                _unitOfWork.Contract.Create(con);
                _unitOfWork.Save();
                return Ok(con);
            }
            return BadRequest();
        }
        [HttpPut("UpdateContract")]
        public IActionResult Update([FromBody] ContractDTO contract) 
        { 
            Contract con = _unitOfWork.Contract.Find(c => c.JopPostId == contract.JopPostId).Where(c => c.IsDeleted == false).FirstOrDefault();
            con.StartDate = contract.StartDate;
            con.EndDate = contract.EndDate;
            con.FreelancerId = contract.FreelancerId;
            con.JopPostId = contract.JopPostId;
            con.ClientId = contract.ClientId;
            con.Price = contract.Price;
            con.PaymentMethodId = contract.PaymentMethodId;
            con.TremsAndCondetions = contract.TremsAndCondetions;
            if (ModelState.IsValid)
            {

                _unitOfWork.Contract.Update(con);
                _unitOfWork.Save();
                return Ok(con);
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
                    _unitOfWork.Contract.Update(conract);
                    _unitOfWork.Save();
                    return Ok(conract);
                }
            }
            return NotFound("This contract already deleted.");
        }

    }
}
