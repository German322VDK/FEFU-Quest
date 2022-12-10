using FEFU_Quest.Domain.Identity;
using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.SQlite.Context;
using Microsoft.EntityFrameworkCore;

namespace FEFU_Quest.Infrastructure.Services
{
    public class UserService : IUser
    {
        private readonly FEFU_QuestDBSQlite _db;
        public UserService(FEFU_QuestDBSQlite db)
        {
            _db = db;
        }

        public UserDTO GetByID(string id) =>
            GetAll().FirstOrDefault(us => us.Id == id);

        public UserDTO Get(string userName) =>
            GetAll().FirstOrDefault(us => us.UserName == userName);

        public IEnumerable<UserDTO> GetAll() =>
            _db.Users.AsNoTracking();

        public UserDTO GetByEmail(string email) =>
            GetAll().FirstOrDefault(us => us.Email == email);
    }
}
