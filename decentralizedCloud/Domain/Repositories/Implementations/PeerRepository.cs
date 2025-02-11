using System.Net;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class PeerRepository: ARepository<Peer>, IPeerRepository 
{
    public PeerRepository(NetworkinfoDbContext context) : base(context)
    {
    }
    
    public async Task<Peer?> FindPeerByIpAndPortAsync(string? ipAddress, int port)
    {
        var peers = await ReadAsync(p => p.IpAddress == ipAddress && p.Port == port);
        return peers.FirstOrDefault();
    }
    
    public async Task<Peer?> GetSuperPeerAsync()
    {
        var peers = await ReadAsync(p => p.peerType=="SUPERPEER");
        return peers.FirstOrDefault();
    }
}