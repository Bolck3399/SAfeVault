using Isopoh.Cryptography.Argon2;

public class Argon2Hasher : IArgon2Hasher
{
    public string HashPassword(string password)
    {
        return Argon2.Hash(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return Argon2.Verify(hash, password);
    }
}