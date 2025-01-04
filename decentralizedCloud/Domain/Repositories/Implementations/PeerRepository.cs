using System.Net;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class PeerRepository: ARepository<Peer>, IPeerRepository 
{
    private readonly IPeerRepository _peerRepository;
    public PeerRepository(DbContext context, PeerRepository peerRepository) : base(context)
    {
        _peerRepository = peerRepository;
    }
    
    public async Task<Peer?> FindPeerByIpAndPortAsync(string? ipAddress, int port)
    {
        var peers = await ReadAsync(p => p.IpAddress == ipAddress && p.Port == port);
        return peers.FirstOrDefault();
    }
    
    public async Task<Peer> GetSuperPeerAsync()
    {
        var peers = await ReadAsync(p => p.IsSuperpeer);
        return peers.FirstOrDefault();
    }
}