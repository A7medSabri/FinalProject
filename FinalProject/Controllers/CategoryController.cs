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
    //[Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [HttpGet("Get-All-Categories-With-Id-Admin")]
        public ActionResult GetAllWithIdForAdmin()
        {
            try
            {
                var result = _unitOfWork.Category.GetAll().Select(s => new { s.Id, s.Name , s.IsDeleted}).ToList();
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

        [HttpGet("Get-All-Category-By-Id")]
        public ActionResult GetCatById(int id)
        {
            try
            {
                var result = _unitOfWork.Category.GetByID(id);

                if (result == null)
                {
                    return NotFound("No Category With This Id");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later: {ex.Message}");
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

                var existingCategory = _unitOfWork.Category.FindCat(cat.name);
                if (existingCategory != null && existingCategory.IsDeleted == false )
                {
                    return BadRequest("Category already exists.");
                }
                if (existingCategory != null && existingCategory.IsDeleted)
                {
                    return BadRequest("Category Is Deleted");
                }

                _unitOfWork.Category.Create(cat);
                _unitOfWork.Save();

                return Ok(cat);
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
                if (oldCat.IsDeleted == true)
                {
                    return Ok("Category Is Deleted, retrun From Delete if You Want Edit It.");
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
                if(cat.IsDeleted)
                {
                    return BadRequest("Is Already deleted");
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
        [HttpPut("Return-Delete-Category")]
        public IActionResult returnFormDelete(int id)
        {
            try
            {
                var cat = _unitOfWork.Category.GetByID(id);
                if (cat == null)
                {
                    return NotFound("Category not found with this ID.");
                }
                if (cat.IsDeleted == false)
                {
                    return BadRequest("Is Already Exit");
                }
                _unitOfWork.Category.returnFromDelete(id);
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
