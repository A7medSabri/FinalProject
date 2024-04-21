using FinalProject.Domain.DTO.Skill;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.SkillAndCat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class SkillController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        //Done
        [AllowAnonymous]
        [HttpGet("Get-All-SKills")]
        public ActionResult GetAll()
        {
            List<string> result = _unitOfWork.Skill.GetAll().Where(s => s.IsDeleted == false).Select(s => s.Name).ToList();
            if (!result.IsNullOrEmpty())
            {
                if(ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }

        //Done
        [AllowAnonymous]
        [HttpGet("Get-All-SKills-With-Id")]
        public ActionResult GetAllWithId()
        {
            var result = _unitOfWork.Skill.GetAll().Where(s=>s.IsDeleted==false).Select(s => new {s.Id , s.Name}).ToList();
            if (!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }

        [HttpGet("Get-All-SKills-With-Id-For-Admin")]
        public ActionResult GetAllWithIdForAdmin()
        {
            var result = _unitOfWork.Skill.GetAll().Select(s => new { s.Id, s.Name ,s.IsDeleted}).ToList();
            if (!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }
        [HttpGet("Get-Skill-By-Id")]
        public IActionResult GetSkillById(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Not Vaild");
            }
            var data = _unitOfWork.Skill.GetByID(id);
            if(data == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(data);
        }
        //Done
        [HttpPost("Add-New-Skill")]
        public ActionResult Add( SkillDto skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input.");
            }

            try
            {
                var data = _unitOfWork.Skill.FindSkill(skill.name);
                if (data != null && data.IsDeleted == false)
                {
                    return BadRequest("Is Exiting");
                }
                if (data != null && data.IsDeleted)
                {
                    return BadRequest("Is Exiting and Deleted");
                }
                var newSkill = _unitOfWork.Skill.Create(skill);
                _unitOfWork.Save();
                return Ok(newSkill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred {ex.Message}.");
            }
        }

        //Done
        [HttpPut("Edit-Skill")]
        public IActionResult Update(int id, SkillDto skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input.");
            }

            var existingSkill = _unitOfWork.Skill.GetByID(id);
            if (existingSkill == null)
            {
                return NotFound("Skill not found.");
            }

            try
            {
                var updatedSkill = _unitOfWork.Skill.Edit(id, skill);
                _unitOfWork.Save();
                return Ok(updatedSkill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred {ex.Message}.");
            }
        }

        //Done
        [HttpPut("Delete-Skill")]
        public IActionResult Delete(int id)
        {
            var skill = _unitOfWork.Skill.GetByID(id);
            if (skill == null)
            {
                return NotFound("Not Found Any Skill With This Id");
            }
            if (skill.IsDeleted)
            {
                return BadRequest("Is Already Deleted");
            }
            _unitOfWork.Skill.Remove(id);
            _unitOfWork.Save();
            return Ok(skill);
        }

        [HttpPut("Return-Delete-Skill")]
        public IActionResult RetuenDeletedSkill(int id)
        {
            var skill = _unitOfWork.Skill.GetByID(id);
            if (skill == null)
            {
                return NotFound("Not Found Any Skill With This Id");
            }
            if (skill.IsDeleted == false)
            {
                return BadRequest("Is Already Exist");
            }
            _unitOfWork.Skill.returnDeletedSkill(id);
            _unitOfWork.Save();
            return Ok(skill);
        }
        //[HttpPut("Edit-Skill")]
        //public IActionResult Update(int id ,SkillDto skill) 
        //{
        //    var oldSkill = _unitOfWork.Skill.GetByID(id);
        //    if(oldSkill == null)
        //        return NotFound("Not Found Any Skill With This Id");

        //    _unitOfWork.Skill.Edit(id, skill);
        //    _unitOfWork.Save();

        //    return Ok(skill);
        //}
    }
}
