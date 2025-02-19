using System.Security.Cryptography;
using System.Text;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class UserRepository:ARepository<User>,IUserRepository
{
    public UserRepository(NetworkinfoDbContext dbContext) : base(dbContext)
    {
        
    }
    
    public async Task<string?> GetUserAccessData(string username, int dataId)
    {
        int userId = await GetUserIdByUsername(username);
        return await _dbSet
            .Where(u => u.UserId == userId)
            // .Include(u => u.DataOwnerships)
            .Select(u => u.DataAccesses.FirstOrDefault(d => d.DataId ==dataId).OwnershipType)
            .FirstOrDefaultAsync();
    }
    public async Task<int> GetUserIdByUsername(string username) => 
        await _dbSet.Where(u => u.Username == username).Select(u => u.UserId).FirstOrDefaultAsync();
    
    
    public async Task<User?> ReadGraphAsync(string username)=> await _dbSet
        // .Include(u=>u.DataOwnerships)
        .Include(d=>d.DataAccesses)
        .FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return (await ReadAsync(u => u.Username == username)).FirstOrDefault();
    }

    public async Task<bool> UsernameExistsAsync(string username)
    => await GetByUsernameAsync(username) is not null;

    // public async Task<User?> LoginAsync(string username, string password)
    // {
    //     var user = await GetByUsernameAsync(username);
    //     if (user == null) return null;
    //
    //     return VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    // }
    //
    // private static (string hash, string salt) GenerateHashAndSalt(string password)
    // {
    //     var salt = Guid.NewGuid().ToString();
    //     var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password + salt)));
    //     return (hash, salt);
    // }
    //
    // private static bool VerifyPassword(string password, string hash, string salt)
    // {
    //     var computedHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password + salt)));
    //     return computedHash == hash;
    // }

    // public async Task<User?> AuthenticateUserAsync(string username, string password)
    // {
    //     var user = await GetByUsernameAsync(username);
    //     if (user == null) return null;
    //     return VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    // }
    
    //
    // public async Task<List<Group>> GetUserGroups(string username)
    // {
    //     int userId = await ConvertUsernameToUserId(username);
    //     return await _dbSet
    //         .Where(u => userId == u.UserId)
    //         .Include(ug => ug.UserGroups)
    //         .ThenInclude(ug => ug.Group)
    //         .SelectMany(u => u.UserGroups.Select(ug => ug.Group))
    //         .ToListAsync();
    // }
    //
}