using FinalProject.Domain.DTO.Skill;
using FinalProject.Domain.DTO.Skill_Lang_Cat;
using FinalProject.Domain.Models.SkillAndCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public CatDto Create(CatDto catDto);
        public Category Remove(int id);
        public CatDto Edit(int id, CatDto catDto);

    }
}
