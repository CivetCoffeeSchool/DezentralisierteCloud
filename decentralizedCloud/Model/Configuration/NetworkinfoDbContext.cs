using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class NetworkinfoDbContext: DbContext
{
    #region DbSets
    public DbSet<Data> Data { get; set; }
    public DbSet<Peer> Peers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<DataOnPeers> DataDistributions { get; set; }
    public DbSet<UserAccessData> DataOwnerships { get; set; }
    #endregion
    
    public NetworkinfoDbContext(DbContextOptions<NetworkinfoDbContext> options) :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<DataOnPeers>().HasKey(dd => new { dd.DataId, dd.PeerMacAddress });
        
        builder.Entity<DataOnPeers>()
            .HasOne(dd => dd.Data)
            .WithMany(d => d.DataDistributions)
            .HasForeignKey(dd => dd.DataId);
        
        builder.Entity<DataOnPeers>()
            .HasOne(dd => dd.Peer)
            .WithMany()
            .HasForeignKey(dd => dd.PeerMacAddress);
        
        builder.Entity<UserAccessData>().HasKey(d => new { d.Username, d.DataId });
        
        builder.Entity<UserAccessData>()
            .HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.Username);
        
        builder.Entity<UserAccessData>()
            .HasOne(d => d.Data)
            .WithMany()
            .HasForeignKey(d=>d.DataId);
        
        builder.Entity<User>()
            .HasDiscriminator(u => u.userType)
            .HasValue<AdminUser>("ADMIN")
            .HasValue<NormalPeer>("USER");
        
        builder.Entity<Peer>()
            .HasDiscriminator(u => u.peerType)
            .HasValue<SuperPeer>("SUPERPEER")
            .HasValue<NormalPeer>("PEER");
        
        builder.Entity<UserAccessData>()
            .HasDiscriminator(d => d.ownerShipType)
            .HasValue<Owner>("OWNER")
            .HasValue<Reader>("READER")
            .HasValue<NoAccess>("NONE");
    }
}