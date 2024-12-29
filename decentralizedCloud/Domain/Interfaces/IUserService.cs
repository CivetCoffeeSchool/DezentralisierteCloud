using Model.Entities;

namespace Domain.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(User user);
    Task<bool> ValidateUserAsync(User user);
}