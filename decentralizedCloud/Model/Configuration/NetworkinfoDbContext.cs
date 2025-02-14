using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configuration;

public class NetworkinfoDbContext: DbContext
{
    #region DbSets
    public DbSet<Data> Data { get; set; }
    public DbSet<Peer> Peers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserHasGroup> UserHasGroups { get; set; }
    public DbSet<DataOnPeers> DataDistributions { get; set; }
    public DbSet<UserAccessData> DataOwnerships { get; set; }
    #endregion
    
    public NetworkinfoDbContext(DbContextOptions<NetworkinfoDbContext> options) :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(u=> u.Username).IsUnique();
        
        builder.Entity<Data>().HasIndex(d => d.Name).IsUnique();
        
        builder.Entity<DataOnPeers>().HasKey(dd => new { dd.DataId, dd.PeerMacAddress });
        
        builder.Entity<DataOnPeers>()
            .HasOne(dd => dd.Data)
            .WithMany(d => d.DataDistributions)
            .HasForeignKey(dd => dd.DataId);
        
        builder.Entity<DataOnPeers>()
            .HasOne(dd => dd.Peer)
            .WithMany(p => p.DataOnPeers)
            .HasForeignKey(dd => dd.PeerMacAddress);
        
        builder.Entity<UserAccessData>().HasKey(d => new { d.UserId, d.DataId });
        
        builder.Entity<UserAccessData>()
            .HasOne(d => d.Data)
            .WithMany()
            .HasForeignKey(d=>d.DataId);
        
        builder.Entity<UserAccessData>()
            .HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId);
        
        builder.Entity<UserHasGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });
        
        builder.Entity<UserHasGroup>()
            .HasOne(ug => ug.Group)
            .WithMany()
            .HasForeignKey(ug => ug.GroupId);
        
        builder.Entity<UserHasGroup>()
            .HasOne(dd => dd.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(dd => dd.UserId);
        
        builder.Entity<Group>()
            .HasIndex(u => u.GroupName).IsUnique();
        
        builder.Entity<GroupData>().HasKey(dd => new { dd.DataId, dd.GroupId });
        
        builder.Entity<GroupData>()
            .HasOne(dd => dd.Data)
            .WithMany()
            .HasForeignKey(dd => dd.DataId);
        
        builder.Entity<GroupData>()
            .HasOne(dd => dd.Group)
            .WithMany(g => g.GroupDatas)
            .HasForeignKey(dd => dd.GroupId);
        
        builder.Entity<GroupData>()
            .HasOne(dd => dd.Data)
            .WithMany()
            .HasForeignKey(dd => dd.DataId);
        
        builder.Entity<GroupData>()
            .HasOne(dd => dd.Group)
            .WithMany()
            .HasForeignKey(dd => dd.GroupId);
        
        
        builder.Entity<Peer>()
            .HasDiscriminator(u => u.peerType)
            .HasValue<SuperPeer>("SUPERPEER")
            .HasValue<NormalPeer>("PEER");
        
        builder.Entity<UserAccessData>()
            .HasDiscriminator(d => d.ownerShipType)
            .HasValue<Owner>("OWNER")
            .HasValue<Reader>("READER")
            .HasValue<NoAccess>("NONE");
        
        builder.Entity<GroupData>()
            .HasDiscriminator(gd => gd.ownershipType)
            .HasValue<OwnerGroup>("OWNER")
            .HasValue<ReaderGroup>("READER")
            .HasValue<NoAccessGroup>("NONE");
        
        builder.Entity<User>()
            .HasDiscriminator(u => u.userType)
            .HasValue<AdminUser>("ADMIN")
            .HasValue<NormalUser>("USER");
        
    }
}