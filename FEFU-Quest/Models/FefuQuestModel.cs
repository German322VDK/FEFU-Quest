using FEFU_Quest.Domain.Quest;

namespace FEFU_Quest.Models
{
    public class FefuQuestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Place { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public int Count { get; set; }


        public static implicit operator FefuQuestModel(FefuQuest fefuQuest)
        {
            return new FefuQuestModel 
            { 
                Id = fefuQuest.Id,
                DateTimeStart = fefuQuest.DateTimeStart,
                DateTimeEnd = fefuQuest.DateTimeEnd,
                Count = fefuQuest.Count,
                Place = fefuQuest.Place,
                Description = fefuQuest.Description,
                Name = fefuQuest.Name,
            };
        }
    }
}
