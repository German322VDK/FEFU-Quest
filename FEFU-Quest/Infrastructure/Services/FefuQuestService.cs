using FEFU_Quest.Domain.Quest;
using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.SQlite.Context;

namespace FEFU_Quest.Infrastructure.Services
{
    public class FefuQuestService : IFefuQuest
    {
        private readonly FEFU_QuestDBSQlite _db;

        public FefuQuestService(FEFU_QuestDBSQlite db)
        {
            _db = db;
        }

        public void Add(FefuQuest quest)
        {
            if (quest is null)
                return;

            using (_db.Database.BeginTransaction())
            {
                _db.FefuQuests.Add(quest);

                _db.SaveChanges();

                _db.Database.CommitTransaction();
            }
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<FefuQuest> Get() =>
            _db.FefuQuests.ToList();

        public FefuQuest GetById(int id) =>
           _db.FefuQuests.FirstOrDefault(el => el.Id == id);

        public IList<FefuQuest> Get(DateTime start, DateTime end) =>
            _db.FefuQuests.Where(el => el.DateTimeStart > start && el.DateTimeEnd < end).OrderBy(el => el.DateTimeStart).ToList();
    }
}
