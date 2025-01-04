using Model.Entities;

namespace Domain.Services;

public interface IPeerService
{
    Task<List<Peer>> AssignPeersForDataAsync(int dataSize);
    Task NotifyPeersToReceiveDataAsync(Peer sourcePeer, List<(Peer destination, int dataSize)> splits);
}