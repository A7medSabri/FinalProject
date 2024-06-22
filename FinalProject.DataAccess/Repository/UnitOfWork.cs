using FinalProject.DataAccess.Data;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.JobPostAndContract;
using Microsoft.AspNetCore.Hosting;

namespace FinalProject.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IPortfolioRepository Portfolio { get; }
        public IJobPostRepository JobPost { get; }
        public IContractRepository Contract {get; }
        public IlangauageRepository language { get; }
        public ICategoryRepository Category { get; }
        public IRepositoryReport Report { get; }
        public IRatingRepository Rating { get; }
        public ISkillsRepository Skill {  get; }
        public IFavoritesRepository Favorites { get; }
        public IFavJobPostRepository FavJob { get; }

        public IApplyTasksRepository ApplyTasks { get; }

        public IChatRepository Chat { get; }
        
        //   public IApplyTasksRepository ApplyTasks { get; }

        public UnitOfWork(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

            JobPost = new JobPostRepository(_context);
            Contract = new ContractRepository(_context);
            JobPost = new JobPostRepository(_context);
            Portfolio = new PortfolioRepository(_context, _webHostEnvironment  );
            Skill = new SkillRepository(_context);
            language = new langauageRepository(_context);
            Category = new CategoryRepository(_context);
            Report = new RepositoryReport(_context);
            ApplyTasks = new ApplyTasksRepository(_context);
            Favorites = new FavoritesRepository(_context);
            FavJob = new FavJobPostRepository(_context);
            Rating = new RatingRepository(_context);
            Chat = new ChatRepository(_context);
        }



        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
