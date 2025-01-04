using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IDataRepository:IRepository<Data>
{
    Task<List<Peer>> GetPeersByDataIdAsync(int dataId);
}