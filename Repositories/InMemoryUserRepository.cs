using tl2_tp6_2024_tomatorivera.Models;

namespace tl2_tp6_2024_tomatorivera.Repositories
{
    public interface IUserRepository 
    {
        User GetUser(string username, string password);
    }

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public InMemoryUserRepository()
        {
            _users = new List<User>
            {
                new User { Id = 1, Username = "admin", Password = "admin", AccessLevel = AccessLevel.Admin },
                new User { Id = 2, Username = "demo", Password = "demo", AccessLevel = AccessLevel.Usuario }
            };
        }

        public User GetUser(string username, string password)
        {
            return _users.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(password, StringComparison.Ordinal)).FirstOrDefault();
        }
    }
}