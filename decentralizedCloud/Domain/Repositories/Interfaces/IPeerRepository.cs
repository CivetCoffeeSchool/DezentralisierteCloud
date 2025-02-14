using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IPeerRepository: IRepository<Peer>
{
    Task<Peer?> FindPeerByIpAndPortAsync(string? ipAddress, int port);
    Task<Peer?> GetSuperPeerAsync();

    Task<bool> GetPeerSavesFile(string ipaddress, int port, int dataId);
}