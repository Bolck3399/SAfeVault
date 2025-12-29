public interface IArgon2Hasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}