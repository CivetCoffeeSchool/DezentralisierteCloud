using Domain.Repositories.Interfaces;
using Model.Entities;

namespace Domain.Services;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly BcryptPasswordHasher _hasher;

    public AuthService(
        IUserRepository userRepo,
        BcryptPasswordHasher hasher)
    {
        _userRepo = userRepo;
        _hasher = hasher;
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user == null) return null;
        
        return _hasher.VerifyPassword(password, user.PasswordHash) 
            ? user 
            : null;
    }
}