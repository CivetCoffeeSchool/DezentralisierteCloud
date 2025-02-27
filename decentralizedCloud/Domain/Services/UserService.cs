/*using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Model.Entities;

namespace Domain.Services;

// Domain/Services/UserService.cs
public class UserService
{
    private readonly IUserRepository _userRepo;
    private readonly BcryptPasswordHasher _hasher;

    public UserService(
        IUserRepository userRepo,
        BcryptPasswordHasher hasher)
    {
        _userRepo = userRepo;
        _hasher = hasher;
    }

    public async Task<User> RegisterUserAsync(string username, string password)
    {
        if (await _userRepo.UsernameExistsAsync(username))
            throw new ConflictException("Username already exists");

        var hash = _hasher.CreateHash(password);
        
        var user = new NormalUser() {
            Username = username,
            PasswordHash = hash,
        };

        await _userRepo.CreateAsync(user);
        return user;
    }
}*/