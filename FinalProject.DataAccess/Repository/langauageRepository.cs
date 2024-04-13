using FinalProject.DataAccess.Data;
using FinalProject.Domain.DTO.Skill_Lang_Cat;
using FinalProject.Domain.IRepository;
using FinalProject.Domain.Models.RegisterNeeded;
using FinalProject.Domain.Models.SkillAndCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DataAccess.Repository
{
    public class langauageRepository : Repository<Language> , IlangauageRepository
    {
        private ApplicationDbContext _context { get; }
        public langauageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public AddNewLangDto Create(AddNewLangDto NewLang)
        {


            var lang = new Language
            {
                Id = NewLang.Id,
                Value = NewLang.Value,
                IsDeleted = false
            };

            _context.Languages.Add(lang);

            return NewLang;
        }

        public Language Remove(string id)
        {
            var Del = _context.Languages.Find(id);
            Del.IsDeleted = true;
            return Del;
        }

        public LangDto Edit(string id, LangDto Lang)
        {
           var oldLang = _context.Languages.Find(id);
           oldLang.Value = Lang.Value;

            return Lang;
        }
        public Language GetByID(string id)
        {
            return _context.Languages.Find(id);
        }
    }
}
