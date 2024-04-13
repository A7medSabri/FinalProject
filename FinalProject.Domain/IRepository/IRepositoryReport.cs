using FinalProject.Domain.DTO.Report;
using FinalProject.Domain.Models.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IRepositoryReport : IRepository<Reports>
    {
        public Reports Remove(int id);
        public ReportDto Create(ReportDto reportDto, string userId);

    }
}
