using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Report;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.ReportModel;
using FinalProject.Domain.Models.SkillAndCat;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class RepositoryReport : Repository<Reports> , IRepositoryReport
    {
        private ApplicationDbContext _context { get; }
        public RepositoryReport(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Reports Remove(int id)
        {
            var result = _context.Reports.Find(id);

            result.IsDeleted = true;
            return result;
        }
        public ReportDto Create (ReportDto reportDto , string userId)
        {

            var NewReport = new Reports
            {
                Type = reportDto.Type,
                Description = reportDto.Description,
                ClientId = userId,
                FreeLanceUserName = reportDto.FreeLanceUserName ?? null,
                ReportDate = DateTime.Now,
                IsDeleted = false,
            };
            _context.Reports.Add(NewReport);

            return reportDto;
        }
    }
}
