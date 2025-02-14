using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IGroupRepository : IRepository<Group>
{
    Task<string?> GetOwnerShipType(int groupId, int dataId);
}