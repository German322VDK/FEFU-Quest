using FEFU_Quest.Domain.Email;
using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.SQlite.Context;

namespace FEFU_Quest.Infrastructure.Services
{
    public class EmailConfirmService : IEmailConfirm
    {
        private readonly FEFU_QuestDBSQlite _db;

        public EmailConfirmService(FEFU_QuestDBSQlite db)
        {
            _db = db;
        }
        public bool Set(string mail, string hash)
        {
            var item = _db.EmailConfirms.Where(el => el.Email == mail);

            if (item.ToList().Count == 0)
            {
                using (_db.Database.BeginTransaction())
                {
                    _db.EmailConfirms.Add(new EmailConfirm
                    {
                        Email = mail,
                        Hash = hash
                    });

                    _db.SaveChanges();

                    _db.Database.CommitTransaction();
                }

                return true;
            }
            else if (item.ToList().Count == 1)
            {
                using (_db.Database.BeginTransaction())
                {
                    _db.EmailConfirms.FirstOrDefault(el => el.Email == mail).Hash = hash;

                    _db.SaveChanges();

                    _db.Database.CommitTransaction();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public ICollection<EmailConfirm> GetAll() =>
            _db.EmailConfirms.ToList();

        public EmailConfirm Get(string mail) =>
          GetAll().SingleOrDefault(el => el.Email == mail);

    }
}
