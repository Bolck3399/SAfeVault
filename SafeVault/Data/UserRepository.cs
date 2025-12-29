public interface IUserRepository
{
    User? GetUser(string username);
    List<User> GetAll();
}

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public UserRepository(IArgon2Hasher hasher)
    {
        _users.Add(new User
        {
            Username = "admin",
            PasswordHash = hasher.HashPassword("Admin123!"),
            Role = "admin"
        });

        _users.Add(new User
        {
            Username = "user",
            PasswordHash = hasher.HashPassword("User123!"),
            Role = "user"
        });
    }

    public User? GetUser(string username) =>
        _users.FirstOrDefault(u => u.Username == username);

    public List<User> GetAll() => _users;
}