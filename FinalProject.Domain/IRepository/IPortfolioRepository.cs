using FinalProject.Domain.DTO.JobPost;
using FinalProject.Domain.DTO.Portfolio;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IPortfolioRepository
    {

        List<ProtfolioGetDto> GetMyPortfolio(string userId);
        Task EditPortfolio(int id, portfolioDto portfolioDto);
        Task<ProtfolioGetDto> GetByIdAsync(int id);
        Task<AddPortfolio> AddPortfolioAsync(AddPortfolio portfolioDto, IFormFile file, string UserId);
        Task<bool> DeletePortfolioAsync(int id);


    }
}
