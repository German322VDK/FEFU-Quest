using FEFU_Quest.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEFU_Quest.Domain.Quest
{
    public class FefuQuest : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Place { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public int Count { get; set; }
    }
}
