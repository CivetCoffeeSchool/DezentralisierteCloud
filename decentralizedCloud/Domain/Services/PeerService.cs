using System.Net.Sockets;
using Domain.Repositories.Interfaces;
using Model.Entities;

namespace Domain.Services;

public interface PeerService:IPeerService
{
    public class PeerService : IPeerService
    {
        private readonly IRepository<Peer> _peerRepository;

        public PeerService(IRepository<Peer> peerRepository)
        {
            _peerRepository = peerRepository;
        }

        public async Task<List<Peer>> GetReachablePeersAsync()
        {
            var peers = await _peerRepository.ReadAllAsync();
            var tasks = peers.Select(async peer =>
            {
                if (await IsPeerReachableAsync(peer))
                {
                    return peer;
                }
                return null;
            }).ToList();

            var results = await Task.WhenAll(tasks);
            return results.Where(peer => peer != null).ToList()!;
        }
        
        private async Task<bool> IsPeerReachableAsync(Peer peer)
        {
            try
            {
                using var client = new TcpClient();
                var connectTask = client.ConnectAsync(peer.IpAddress, peer.Port);
                var timeoutTask = Task.Delay(3000); // 3-second timeout

                if (await Task.WhenAny(connectTask, timeoutTask) == connectTask)
                {
                    return client.Connected;
                }
            }
            catch
            {
                // Log error (optional)
            }
            return false;
        }
        
        public async Task<List<Peer>> AssignPeersForDataAsync(int dataSize)
        {
            // Fetch all available peers
            var peers = await GetReachablePeersAsync();

            // Filter peers based on sufficient available space
            var eligiblePeers = peers.Where(p => p.TotalSpace >= dataSize).ToList();

            if (eligiblePeers.Count < 2)
                throw new InvalidOperationException("Not enough peers available for data distribution.");

            // Select 2 peers for storing parts of the file
            var selectedPeers = eligiblePeers.Take(2).ToList();

            return selectedPeers;
        }

        public async Task NotifyPeersToReceiveDataAsync(Peer sourcePeer, List<(Peer destination, int dataSize)> splits)
        {
            foreach (var (destination, size) in splits)
            {
                // Notify the source peer to send data to destination peer
                Console.WriteLine(
                    $"Notify {sourcePeer.IpAddress}:{sourcePeer.Port} to send {size} bytes to {destination.IpAddress}:{destination.Port}");
                // In production, implement proper peer-to-peer communication here.
                await Task.CompletedTask;
            }
        }
    }
}