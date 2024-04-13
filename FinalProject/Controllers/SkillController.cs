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
    //[Authorize(Roles = "Admin")]

    public class SkillController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet("Get_All_SKills")]
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
        [HttpPost("Add-New-Skill")]
        public ActionResult Add(SkillDto skill)
        {
            _unitOfWork.Skill.Create(skill);

            _unitOfWork.Save();
            return Ok(skill);
        }
        [HttpDelete("Delete-Skill")]
        public IActionResult Delete(int id)
        {
            var skill = _unitOfWork.Skill.GetByID(id);
            if(skill == null)
                return NotFound("Not Found Any Skill With This Id");
            _unitOfWork.Skill.Remove(id);
            _unitOfWork.Save();
            return Ok(skill);
        }
        [HttpPut("Edit-Skill")]
        public IActionResult Update(int id ,SkillDto skill) 
        {
            var oldSkill = _unitOfWork.Skill.GetByID(id);
            if(oldSkill == null)
                return NotFound("Not Found Any Skill With This Id");
            
            _unitOfWork.Skill.Edit(id, skill);
            _unitOfWork.Save();

            return Ok(skill);
        }
    }
}
