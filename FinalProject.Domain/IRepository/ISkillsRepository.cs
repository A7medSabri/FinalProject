using FinalProject.Domain.DTO.Skill;
using FinalProject.Domain.Models.SkillAndCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface ISkillsRepository : IRepository<Skill>
    {
        public SkillDto Create(SkillDto skill);
        public Skill Remove(int id);
        public SkillDto Edit(int id, SkillDto Skill);
    }
}
