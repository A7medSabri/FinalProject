using FinalProject.DataAccess.Repository;
using FinalProject.Domain.DTO.Skill;
using FinalProject.Domain.DTO.Skill_Lang_Cat;
using FinalProject.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet("Get-All-Categories")]
        public ActionResult GetAll()
        {
            List<string> result = _unitOfWork.Category.GetAll().Where(p => p.IsDeleted == false).Select(s => s.Name).ToList();
            if (!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }
        [HttpGet("Get-All-Categories-With-Id")]
        public ActionResult GetAllWithId()
        {
            var result = _unitOfWork.Category.GetAll().Where(p=>p.IsDeleted == false).Select(s => new { s.Id, s.Name }).ToList();
            if (!result.IsNullOrEmpty())
            {
                if (ModelState.IsValid)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }
        [HttpPost("Add-New-Category")]
        public ActionResult Add(CatDto cat)
        {
            _unitOfWork.Category.Create(cat);

            _unitOfWork.Save();
            return Ok(cat);
        }
        [HttpDelete("Delete-Category")]
        public IActionResult Delete(int id)
        {
            var cat = _unitOfWork.Category.GetByID(id);
            if (cat == null)
                return NotFound("Not Found Any Category With This Id");
            _unitOfWork.Category.Remove(id);
            _unitOfWork.Save();
            return Ok(cat);
        }
        [HttpPut("Edit-Category")]
        public IActionResult Update(int id, CatDto cat)
        {
            var oldCat = _unitOfWork.Category.GetByID(id);
            if (oldCat == null)
                return NotFound("Not Found Any Category With This Id");

            _unitOfWork.Category.Edit(id, cat);
            _unitOfWork.Save();

            return Ok(cat);
        }
    }
}
