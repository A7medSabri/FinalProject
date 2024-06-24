using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Skill_Lang_Cat;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.SkillAndCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        private ApplicationDbContext _context { get; }
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public CatDto Create(CatDto catDto)
        {
            var NewCat = new Category
            {
                Name = catDto.name,
                IsDeleted = false
            };
            _context.Categories.Add(NewCat);

            return catDto;
        }

        public Category Remove(int id)
        {
            var cat = _context.Categories.Find(id);
            cat.IsDeleted = true;
            return cat;
        }

        public CatDto Edit(int id, CatDto catDto)
        {
            var oldCat = _context.Categories.Find(id);
            oldCat.Name = catDto.name;
            return catDto;
        }

        public Category FindCat(string name)
        {
            return _context.Categories.FirstOrDefault(a => a.Name == name);
        }

        public Category returnFromDelete(int id)
        {
            var cat = _context.Categories.Find(id);
            cat.IsDeleted = false;
            return cat;
        }
}
}
