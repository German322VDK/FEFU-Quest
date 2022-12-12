using FEFU_Quest.Domain.Identity;
using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.SQlite.Context;
using Microsoft.EntityFrameworkCore;

namespace FEFU_Quest.Infrastructure.Services
{
    public class UniverGroupService : IUniverGroup
    {
        private readonly FEFU_QuestDBSQlite _db;
        public UniverGroupService(FEFU_QuestDBSQlite db)
        {
            _db = db;
        }

        public bool Add(string name)
        {
            var group = GetName(name);

            if (group is not  null)
                return false;


            using (_db.Database.BeginTransaction())
            {
                _db.UniverGroups
                    .Add(new UniverGroup 
                    { 
                        GroupName = name
                    });

                _db.SaveChanges();

                _db.Database.CommitTransaction();
            }


            return true;
        }

        public IEnumerable<UniverGroup> GetAllT() =>
            _db.UniverGroups.AsNoTracking();

        public UniverGroup GetByIdT(int Id) =>
            GetAll().FirstOrDefault(gr => gr.Id == Id);

        public UniverGroup GetNameT(string name) =>
            GetAll().FirstOrDefault(gr => gr.GroupName == name);

        public IEnumerable<UniverGroup> GetAll() =>
            _db.UniverGroups;

        public UniverGroup GetById(int Id) =>
            GetAll().FirstOrDefault(gr => gr.Id == Id);

        public UniverGroup GetName(string name) =>
            GetAll().FirstOrDefault(gr => gr.GroupName == name);
    }
}
