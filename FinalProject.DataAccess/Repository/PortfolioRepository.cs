using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Portfolio;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ProtfolioModle;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PortfolioRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<ProtfolioGetDto> GetMyPortfolio(string userId)
        {
            var Portfolio = _context.Protfolios
                .Where(p => p.UserId == userId)
                .ToList();

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var portfolioDtos = Portfolio.Select(p => new ProtfolioGetDto
            {
                id = p.Id,
                Name = p.Name,
                Description = p.Description,
                URL = p.URL ?? "",
                Media = string.IsNullOrEmpty(p.Media) ? "" : Path.Combine(wwwRootPath, "ProtfolioMedia", p.Media),
                ProjectDate = p.ProjectDate
            }).ToList();

            return portfolioDtos;
        }

        public async Task<ProtfolioGetDto> GetByIdAsync(int id)
        {
            var portfolio = await _context.Protfolios.FindAsync(id);

            if (portfolio == null)
            {
                return null;
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var portfolioDto = new ProtfolioGetDto
            {
                Name = portfolio.Name,
                Description = portfolio.Description,
                URL = portfolio.URL ?? "",
                Media = string.IsNullOrEmpty(portfolio.Media) ? "" : Path.Combine(wwwRootPath, "ProtfolioMedia", portfolio.Media),
                UserId = portfolio.UserId
            };

            return portfolioDto;
        }

        public async Task<AddPortfolio> AddPortfolioAsync(AddPortfolio portfolioDto, IFormFile file , string UserId)
        {
            // التحقق من الملف
            if (file == null || file.Length == 0)
            {
                Console.WriteLine("No file provided or file is empty.");
                return null;
            }

            // إنشاء مسار فريد للملف
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(wwwRootPath, "ProtfolioMedia", fileName);

            try
            {
                // نسخ الملف إلى المسار المطلوب
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
                portfolioDto.Media = fileName;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file copy or database operations: {ex.Message}");
                return null;
            }

            var portfolio = new Protfolio
            {
                Media = fileName,
                Name = portfolioDto.Name,
                Description = portfolioDto.Description,
                URL = portfolioDto.URL,
                UserId = UserId,
                ProjectDate = portfolioDto.ProjectDate
            };

            // إضافة Protfolio إلى السياق
            _context.Protfolios.Add(portfolio);
            await _context.SaveChangesAsync();

            return portfolioDto;

        }


        public async Task EditPortfolio(int id, portfolioDto portfolioDto)
        {
            var portfolio = await _context.Protfolios.FindAsync(id);

            portfolio.Name = portfolioDto.Name;
            portfolio.Description = portfolioDto.Description;
            portfolio.URL = portfolioDto.URL;
            portfolio.Media = portfolioDto.Media;
            portfolio.ProjectDate = portfolioDto.ProjectDate;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePortfolioAsync(int id)
        {
            var portfolio = await _context.Protfolios.FindAsync(id);
            if (portfolio == null)
            {
                return false; 
            }

            _context.Protfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return true; 
        }

    }
}
