using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<List<Group>> GetUserGroups(string username);
    Task<string?> GetUserAccessData(string username, int dataId);
    
    
    Task<User?> ReadGraphAsync(string username);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> RegisterAsync(string username, string password, string userType = "USER");
    Task<User?> LoginAsync(string username, string password);
    // Task<bool> ValidateOwnershipAsync(string username, int dataId);
    // Task<List<Data>> GetUserFilesAsync(string username);
    
    Task<User?> AuthenticateUserAsync(string username, string password);
    //
    // Task<List<Data>> GetDownloadableDataAsync(string username);
    // Task<List<Data>> GetDownloadableDataAsync(User user);
    // Task<List<Data>> GetManageableDataAsync(string username);
    // Task<List<Data>> GetManageableDataAsync(User user);
}