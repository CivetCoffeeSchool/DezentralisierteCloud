using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IDataOwnershipRepository
{
    public Task<List<DataOwnership>> GetByUsernameAsync(string username);
    public Task<DataOwnership?> GetOwnershipAsync(string username, int dataId);
}