using MovieVault.Data.Models;

namespace MovieVault.Core.Services
{
    public static class UserSession
    {
        private static User? _currentUser;

        public static bool IsLoggedIn => _currentUser != null;
        public static User? CurrentUser => _currentUser;

        public static void SetUser(User user)
        {
            _currentUser = user;
        }

        public static void Clear()
        {
            _currentUser = null;
        }
    }

}
