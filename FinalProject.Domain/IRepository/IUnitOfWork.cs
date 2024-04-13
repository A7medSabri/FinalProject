
namespace FinalProject.Domain.IRepository
{    
    public interface IUnitOfWork 
    {
        IJobPostRepository JobPost { get; }
        IPortfolioRepository Portfolio{ get; }
        IContractRepository Contract { get; } 
        ISkillsRepository Skill { get; }
        IlangauageRepository language { get; }
        ICategoryRepository Category { get; }
        IRepositoryReport Report { get; }
        IApplyTasksRepository ApplyTasks { get; }

        void Save();
        
    }
}
