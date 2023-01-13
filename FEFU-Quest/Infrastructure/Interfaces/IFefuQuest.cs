using FEFU_Quest.Domain.Quest;

namespace FEFU_Quest.Infrastructure.Interfaces
{
    public interface IFefuQuest
    {
        public FefuQuest GetById(int id);

        public IList<FefuQuest> Get();

        public IList<FefuQuest> Get(DateTime start, DateTime end);

        public bool DeleteById(int id);

        public void Add(FefuQuest quest);
    }
}
