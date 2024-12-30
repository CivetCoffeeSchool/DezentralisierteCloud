using System.Security.Cryptography;
using System.Text;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class UserRepository:ARepository<User>,IUserRepository
{
    
    private readonly IDataOwnershipRepository _ownershipRepository;

    public UserRepository(DbContext dbContext, IDataOwnershipRepository ownershipRepository) : base(dbContext)
    {
        _ownershipRepository = ownershipRepository;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return (await ReadAsync(u => u.Username == username)).FirstOrDefault();
    }


    public async Task<User?> RegisterAsync(string username, string password, bool isAdmin = false)
    {
        // var existingUser = await GetByUsernameAsync(username);
        // if (existingUser != null) return null;
        if (await ExistsAsync(u=>u.Username == username)) return null;
        
        var (hash, salt) = GenerateHashAndSalt(password);
        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt,
            IsAdmin = isAdmin
        };
        return await CreateAsync(user);
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user == null) return null;

        return VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    }

    public async Task<bool> ValidateOwnershipAsync(string username, int dataId)
    {
        var ownership = await _ownershipRepository.GetOwnershipAsync(username, dataId);
        return ownership != null;
    }

    public async Task<List<Data>> GetUserFilesAsync(string username)
    {
        var ownerships = await _ownershipRepository.GetByUsernameAsync(username);
        return ownerships.Select(o => o.Data).ToList();
    }

    private static (string hash, string salt) GenerateHashAndSalt(string password)
    {
        var salt = Guid.NewGuid().ToString();
        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password + salt)));
        return (hash, salt);
    }

    private static bool VerifyPassword(string password, string hash, string salt)
    {
        var computedHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password + salt)));
        return computedHash == hash;
    }
}