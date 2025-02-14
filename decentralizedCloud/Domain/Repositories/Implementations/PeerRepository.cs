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
    
    public async Task<List<DataOnPeers>> PeerSavesFileAsync(string ipaddress, int port, int dataId) => await _dbSet
        .Where(p => p.IpAddress == ipaddress && p.Port == port)
        .Include(d => d.DataOnPeers)
        .Select(p => p.DataOnPeers).SelectMany(dp => dp
            .Where(d => dataId == d.DataId))
        .ToListAsync();
    
    public async Task<bool> GetPeerSavesFile(string ipaddress, int port,int dataId)
    {
        // Check if the Peer already saves a part of the file 
        Console.WriteLine($"Calling GetPeerSavesFile for dataId {dataId} on {ipaddress}:{port}");

        if ((await PeerSavesFileAsync(ipaddress, port, dataId)).Count > 0)
        {
            return true;
        }
        return false;
    }

}