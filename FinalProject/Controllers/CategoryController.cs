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
            try
            {
                var result = _unitOfWork.Category.GetAll().Where(p => p.IsDeleted == false).Select(s => s.Name).ToList();
                if (!result.IsNullOrEmpty() && ModelState.IsValid)
                {
                    return Ok(result);
                }
                return NotFound("No categories found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("Get-All-Categories-With-Id")]
        public ActionResult GetAllWithId()
        {
            try
            {
                var result = _unitOfWork.Category.GetAll().Where(p => p.IsDeleted == false).Select(s => new { s.Id, s.Name }).ToList();
                if (!result.IsNullOrEmpty() && ModelState.IsValid)
                {
                    return Ok(result);
                }
                return NotFound("No categories found with IDs.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later{ex.Message}");
            }
        }

        [HttpPost("Add-New-Category")]
        public IActionResult Add(CatDto cat)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var existingCategory = _unitOfWork.Category.Find(u => u.Name == cat.name);
                if (existingCategory != null)
                {
                    return BadRequest("Category already exists.");
                }

                _unitOfWork.Category.Create(cat);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later{ex.Message}");
            }
        }

        [HttpPut("Edit-Category")]
        public IActionResult Update(int id, CatDto cat)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var oldCat = _unitOfWork.Category.GetByID(id);
                if (oldCat == null)
                {
                    return NotFound("Category not found with this ID.");
                }

                _unitOfWork.Category.Edit(id, cat);
                _unitOfWork.Save();

                return Ok(cat);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpDelete("Delete-Category")]
        public IActionResult Delete(int id)
        {
            try
            {
                var cat = _unitOfWork.Category.GetByID(id);
                if (cat == null)
                {
                    return NotFound("Category not found with this ID.");
                }

                _unitOfWork.Category.Remove(id);
                _unitOfWork.Save();

                return Ok(cat);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        //[HttpPost("Add-New-Category")]
        //public ActionResult Add(CatDto cat)
        //{
        //    _unitOfWork.Category.Create(cat);

        //    _unitOfWork.Save();
        //    return Ok(cat);
        //}
        //[HttpPut("Edit-Category")]
        //public IActionResult Update(int id, CatDto cat)
        //{
        //    var oldCat = _unitOfWork.Category.GetByID(id);
        //    if (oldCat == null)
        //        return NotFound("Not Found Any Category With This Id");

        //    _unitOfWork.Category.Edit(id, cat);
        //    _unitOfWork.Save();

        //    return Ok(cat);
        //}
    }
}
