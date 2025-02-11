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
    public DbSet<DataDistribution> DataDistributions { get; set; }
    public DbSet<DataOwnership> DataOwnerships { get; set; }
    #endregion
    
    public NetworkinfoDbContext(DbContextOptions<NetworkinfoDbContext> options) :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<DataDistribution>()
            .HasKey(dd => new { dd.DataId, dd.PeerMacAddress });
        builder.Entity<DataDistribution>()
            .HasOne(dd => dd.Data)
            .WithMany()
            .HasForeignKey(dd => dd.DataId);
        builder.Entity<DataDistribution>()
            .HasOne(dd => dd.Peer)
            .WithMany()
            .HasForeignKey(dd => dd.PeerMacAddress);

        builder.Entity<DataOwnership>()
            .HasKey(d => new { d.Username, d.DataId });
        builder.Entity<DataOwnership>()
            .HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.Username);
        builder.Entity<DataOwnership>()
            .HasOne(d => d.Data)
            .WithMany()
            .HasForeignKey(d=>d.DataId);
        
        builder.Entity<User>()
            .HasDiscriminator(u => u.userType)
            .HasValue("ADMIN")
            .HasValue("USER");
        
        builder.Entity<Peer>()
            .HasDiscriminator(u => u.peerType)
            .HasValue("SUPERPEER")
            .HasValue("PEER");
        
        builder.Entity<DataOwnership>()
            .HasDiscriminator(d => d.ownerShipType)
            .HasValue("OWNER")
            .HasValue("READER")
            .HasValue("NONE");
    }
}