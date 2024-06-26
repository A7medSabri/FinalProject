﻿using FinalProject.Domain.DTO.Skill;
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
    [Authorize(Roles = "Admin")]

    public class LanguageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LanguageController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("Get-All-Language")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _unitOfWork.language.GetAll()
                    .Where(lang => lang.IsDeleted == false)
                    .Select(lang => lang.Value)
                    .ToList();

                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound("No languages found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [AllowAnonymous]
        [HttpGet("Get-All-Language-With-Id")]
        public IActionResult GetAllWithId()
        {
            try
            {
                var result = _unitOfWork.language.GetAll()
                    .Where(lang => lang.IsDeleted == false)
                    .Select(lang => new { lang.Id, lang.Value })
                    .ToList();

                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound("No languages found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("Get-All-Language-With-Id-For-Admin")]
        public IActionResult GetAllWithIdForAdmin()
        {
            try
            {
                var result = _unitOfWork.language.GetAll()
                    .Select(lang => new { lang.Id, lang.Value ,lang.IsDeleted})
                    .ToList();

                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound("No languages found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }
        [HttpPost("Add-New-Language")]
        public IActionResult Add(AddNewLangDto lang)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }
                var IsFounded = _unitOfWork.language.GetByID(lang.Id);
                var existingLanguage = _unitOfWork.language.FindLanguage(lang.Value);

                if (IsFounded != null || (existingLanguage != null && !existingLanguage.IsDeleted))
                {
                    return BadRequest("Id or Language already exists.");
                }
                if (existingLanguage != null && existingLanguage.IsDeleted)
                {
                    return BadRequest("Language already exists and Deleted.");
                }

                _unitOfWork.language.Create(lang);
                _unitOfWork.Save();

                return Ok(lang);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }

        [HttpPut("Edit-Language")]
        public IActionResult Edit(string id, LangDto langDto)
        {
            try
            {
                var existingLanguage = _unitOfWork.language.GetByID(id);
                if (existingLanguage == null)
                {
                    return NotFound("Language not found with this ID.");
                }

                var editedLang = _unitOfWork.language.Edit(id, langDto);
                _unitOfWork.Save();

                return Ok(editedLang);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }

        [HttpPut("Delete-Language")]
        public IActionResult Delete(string id)
        {
            try
            {
                var lang = _unitOfWork.language.GetByID(id);
                if (lang == null)
                {
                    return NotFound("Language not found with this ID.");
                }
                if(lang.IsDeleted)
                {
                    return BadRequest("Language Is Deleted.");
                }

                _unitOfWork.language.Remove(id);
                _unitOfWork.Save();

                return Ok(lang);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }

        [HttpPut("Return-Delete-Language")]
        public IActionResult ReturnFromDeleted(string id)
        {
            try
            {
                var lang = _unitOfWork.language.GetByID(id);
                if (lang == null)
                {
                    return NotFound("Language not found with this ID.");
                }
                if (lang.IsDeleted == false)
                {
                    return BadRequest("Language Is Exist.");
                }

                _unitOfWork.language.returnDeletedLang(id);
                _unitOfWork.Save();

                return Ok(lang);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred. Please try again later {ex.Message}.");
            }
        }


    }
}
