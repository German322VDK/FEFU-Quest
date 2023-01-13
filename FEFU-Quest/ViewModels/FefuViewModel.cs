using FEFU_Quest.Domain.Quest;
using FEFU_Quest.Models;

namespace FEFU_Quest.ViewModels
{
    public class FefuViewModel
    {
        public string Week { get; set; }

        public IList<FefuQuestModel>[] Quests { get; set; } = new IList<FefuQuestModel>[7];
    }
}
