namespace Domain.Services;

public interface IPasswordHasher
{
    // Domain/Services/IPasswordHasher.cs
    public interface IPasswordHasher
    {
        (string Hash, string Salt) CreateHash(string password);
        bool VerifyPassword(string password, string hash, string salt);
    }
}