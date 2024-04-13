using FinalProject.DataAccess.Data;
using FinalProject.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinalProject.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ApplicationDbContext _context;
         public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(T Entity)
        {
            _context.Set<T>().Add(Entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Delete(T Entity)
        {
            _context.Set<T>().Remove(Entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
                return _context.Set<T>().Where(expression);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetByID(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public T Include(Expression<Func<T, object>> includeProperties, Expression<Func<T, bool>> id)
        {
            return _context.Set<T>().Include(includeProperties).FirstOrDefault(id);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }


    }
}
