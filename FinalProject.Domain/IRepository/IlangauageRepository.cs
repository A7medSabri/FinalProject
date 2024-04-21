using FinalProject.Domain.DTO.Skill;
using FinalProject.Domain.DTO.Skill_Lang_Cat;
using FinalProject.Domain.Models.RegisterNeeded;
using FinalProject.Domain.Models.SkillAndCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Domain.IRepository
{
    public interface IlangauageRepository : IRepository<Language>
    {
        public AddNewLangDto Create(AddNewLangDto Lang);
        public Language Remove(string id);
        public LangDto Edit(string id, LangDto Lang);
        public Language GetByID(string id);
        public Language FindLanguage(string name);
    }
}
