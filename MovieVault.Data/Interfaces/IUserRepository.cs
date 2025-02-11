using MovieVault.Data.Models;

namespace MovieVault.Data.Interfaces;

public interface IUserRepository
{
    User GetUserById(int userId);
    User GetUserByEmail(string email);
    bool CreateUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(int userId);
}