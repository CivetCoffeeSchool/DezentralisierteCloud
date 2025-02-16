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
    
    [Column("TOTAL_SPACE")]
    public int TotalSpace { get; set; }
    
    [Column("IP_ADDRESS")]
    [StringLength(15)]
    public string IpAddress { get; set; }
    
    [Column("PORT")]
    public int Port { get; set; }

    public List<DataOnPeers> DataOnPeers { get; set; }

}