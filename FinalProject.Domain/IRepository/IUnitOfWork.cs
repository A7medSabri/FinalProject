
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
        IRatingRepository Rating { get; }
        IFavoritesRepository Favorites { get; }
        IFavJobPostRepository FavJob { get; }
        IApplyTasksRepository ApplyTasks { get; }
        IChatRepository Chat { get; }
        IPaymentTestRepository PayTest { get; }
        void Save();
        
    }
}
