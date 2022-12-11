using FEFU_Quest.Domain.Email;

namespace FEFU_Quest.Infrastructure.Interfaces
{
    public interface IEmailConfirm
    {
        public EmailConfirm Get(string mail);

        public ICollection<EmailConfirm> GetAll();

        public bool Set(string mail, string hash);
    }
}
