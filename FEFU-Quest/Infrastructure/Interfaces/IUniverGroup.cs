using FEFU_Quest.Domain.Identity;

namespace FEFU_Quest.Infrastructure.Interfaces
{
    public interface IUniverGroup
    {
        public IEnumerable<UniverGroup> GetAllT();

        public UniverGroup GetByIdT(int Id);

        public UniverGroup GetNameT(string name);

        public IEnumerable<UniverGroup> GetAll();

        public UniverGroup GetById(int Id);

        public UniverGroup GetName(string name);

        public bool Add(string name);
    }
}
