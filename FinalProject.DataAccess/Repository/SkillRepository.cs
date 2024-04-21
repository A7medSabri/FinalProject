using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Skill;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.SkillAndCat;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class SkillRepository : Repository<Skill>, ISkillsRepository
    {
        private ApplicationDbContext _context { get; }
        public SkillRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public SkillDto Create(SkillDto skill)
        {
            var Nskill = new Skill
            {
                Name = skill.name,
                IsDeleted = false
            };
            _context.Skills.Add(Nskill);

            return skill;
        }
        public Skill Remove(int id)
        {
            var Del = _context.Skills.Find(id);
            Del.IsDeleted = true;
            return Del;
        }

        public SkillDto Edit(int id , SkillDto Skill)
        {
            var skill = _context.Skills.Find(id);
            skill.Name = Skill.name;

            return Skill;
        }
        public Skill FindSkill(string name)
        {
            return _context.Skills.FirstOrDefault(skill => skill.Name == name);
        }

        public Skill returnDeletedSkill(int id)
        {
            var Del = _context.Skills.Find(id);
            Del.IsDeleted = false;
            return Del;
        }
    }
}
