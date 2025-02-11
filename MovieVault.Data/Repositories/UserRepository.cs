using MovieVault.Data.Interfaces;
using MovieVault.Data.Models;

namespace MovieVault.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
