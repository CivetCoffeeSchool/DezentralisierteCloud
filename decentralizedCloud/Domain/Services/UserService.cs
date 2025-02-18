using Domain.Repositories.Interfaces;
using Model.Entities;

namespace Domain.Services;

// Domain/Services/UserService.cs
public class UserService
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _hasher;

    public UserService(
        IUserRepository userRepo,
        IPasswordHasher hasher)
    {
        _userRepo = userRepo;
        _hasher = hasher;
    }

    public async Task<User> RegisterUserAsync(string username, string password)
    {
        if (await _userRepo.UsernameExistsAsync(username))
            throw new ConflictException("Username already exists");

        var (hash, salt) = _hasher.CreateHash(password);
        
        var user = new User {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        await _userRepo.CreateAsync(user);
        return user;
    }
}