using FinalProject.Domain.DTO.Skill;
using FinalProject.Domain.DTO.Skill_Lang_Cat;
using FinalProject.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]

    public class LanguageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LanguageController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet("Get-All-Language")]
        public ActionResult GetAll()
        {
            List<string> result = _unitOfWork.language.GetAll().Where(s => s.IsDeleted == false).Select(s => s.Value).ToList();
            if (!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }
        [HttpGet("Get-All-Language-With-Id")]
        public ActionResult GetAllWithId()
        {
            var result = _unitOfWork.language.GetAll().Where(s=>s.IsDeleted==false).Select(s => new { s.Id, s.Value }).ToList();
            if (!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }
        [HttpPost("Add-New-Language")]
        public ActionResult Add(AddNewLangDto lang)
        {
            var existingLanguage = _unitOfWork.language.GetByID(lang.Id);

            if (existingLanguage != null)
            {
                return BadRequest($"Language with Id already exists.");
            }
            _unitOfWork.language.Create(lang);

            _unitOfWork.Save();
            return Ok(lang);
        }

        [HttpDelete("Delete-Language")]
        public IActionResult Delete(string id)
        {
            var lang = _unitOfWork.language.GetByID(id);
            if (lang == null)
                return NotFound("Not Found Any Language With This Id");
            _unitOfWork.language.Remove(id);
            _unitOfWork.Save();
            return Ok(lang);
        }
        [HttpPut("Edit-Language")]
        public IActionResult Update(string id, LangDto lang)
        {
            var oldLang = _unitOfWork.language.GetByID(id);
            if (oldLang == null)
                return NotFound("Not Found Any Language With This Id");

            _unitOfWork.language.Edit(id, lang);
            _unitOfWork.Save();

            return Ok(lang);
        }
    }
}
