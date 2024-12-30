using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByUsernameAsync(string username);
    Task<User?> RegisterAsync(string username, string password, bool isAdmin = false);
    Task<User?> LoginAsync(string username, string password);
    Task<bool> ValidateOwnershipAsync(string username, int dataId);
    Task<List<Data>> GetUserFilesAsync(string username);
}