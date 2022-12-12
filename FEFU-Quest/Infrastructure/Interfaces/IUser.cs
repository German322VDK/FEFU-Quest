using FEFU_Quest.Domain.Identity;

namespace FEFU_Quest.Infrastructure.Interfaces
{
    public interface IUser
    {
        public IEnumerable<UserDTO> GetAll();

        public UserDTO GetByID(string id);

        public UserDTO Get(string userName);

        public UserDTO GetByEmail(string email);

        public bool AddPhoto(byte[] image, string userName);
    }
}
