using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("PEERS_ST")]
public class Peer
{
    [Column("PEER_ID")] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
    public int PeerId { get; set; }
    
    [Column("PEER_TYPE"),StringLength(45)]
    public string PeerType { get; set; }
    
    [Column("AVALIABLE_SPACE")]
    public long AvaliableSpace { get; set; }
    
    [Column("IP_ADDRESS")]
    [StringLength(15)]
    public string IpAddress { get; set; }
    
    [Column("PORT")]
    public int Port { get; set; }
    
    [Column("LAST_HEARTBEAT")]
    public DateTimeOffset LastHeartbeat { get; set; }

    public List<DataOnPeers> DataOnPeers { get; set; } = new List<DataOnPeers>();

}