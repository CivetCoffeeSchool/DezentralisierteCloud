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
    #endregion
    
    public NetworkinfoDbContext(DbContextOptions<NetworkinfoDbContext> options) :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<DataDistribution>()
            .HasKey(dd => new { dd.DataId, dd.PeerId });
        builder.Entity<DataDistribution>()
            .HasOne(dd => dd.Data)
            .WithMany()
            .HasForeignKey(dd => dd.DataId);
        builder.Entity<DataDistribution>()
            .HasOne(dd => dd.Peer)
            .WithMany()
            .HasForeignKey(dd => dd.PeerId);

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
    }
}