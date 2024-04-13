using FinalProject.Domain.DTO.Report;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ReportModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //[Authorize(Roles ="Admin")]
        [HttpGet("Get-All-Reports-For-Admin")]
        public IActionResult GetAllRepotsForAdmin()
        {
            var result =  _unitOfWork.Report.GetAll();
            if(!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound("No Reports");
        }

        [HttpGet("Get-My-Reports")]
        public IActionResult GetMyRepots()
        {
            var userId = User.FindFirst("uid")?.Value;

            var result = _unitOfWork.Report.GetAll()
                .Where(s=> s.IsDeleted == false )
                .Where(s=> s.ClientId == userId).ToList();
            if (!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound("No Reports");
        }

        [HttpGet("Get-Report-By-Id")]
        public IActionResult GetRepotbyId(int id) 
        {
            var userId = User.FindFirst("uid")?.Value;
            var result = _unitOfWork.Report.GetByID(id);

            if(result != null && result.ClientId == userId)
            {
                return Ok(result);
            }


            return NotFound("No Report With This Id");
        }
        [HttpPost("New-Report")]
        public IActionResult Create(ReportDto reportDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            _unitOfWork.Report.Create(reportDto , userId);
            _unitOfWork.Save();
            return Ok(reportDto);
        }

            
        [HttpPut("Edit-Report")]
        public IActionResult Edit(int id , ReportDto reportDto)
        {
            var userId = User.FindFirst("uid")?.Value;
            var oldReport = _unitOfWork.Report.GetByID(id);
            if(oldReport == null)
            {
                return NotFound("Not Report With This id");
            }
            oldReport.Type = reportDto.Type;
            oldReport.Description = reportDto.Description;
            oldReport.FreeLanceUserName = reportDto.FreeLanceUserName ?? null;
            oldReport.Type = reportDto.Type;

            return Ok(oldReport);
        }


        [HttpDelete("Delete-Report")]
        public IActionResult Delete(int id)
        {
            var result = _unitOfWork.Report.GetByID(id);
            var userId = User.FindFirst("uid")?.Value;

            
            if (result != null && result.ClientId == userId)
            {
                _unitOfWork.Report.Remove(id);
                _unitOfWork.Save();
                return Ok(result);
            }

            return NotFound("No Report With This Id");
        }

    }
}
