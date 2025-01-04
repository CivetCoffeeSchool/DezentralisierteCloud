using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IPeerRepository: IRepository<Peer>
{
    Task<Peer?> FindPeerByIpAndPortAsync(string? ipAddress, int port);
    Task<Peer> GetSuperPeerAsync();
}