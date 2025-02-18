using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("DATA_SAVED_ON_PEERS_JT")]
public class DataOnPeers
{
    [Column("SEQUENCE_NUMBER"),Key]
    public int SequenceNumber { get; set; }
    
    [Column("DATA_ID"),Required]
    public int DataId { get; set; }
    
    [Column("PEER_ID"),Required]
    public int PeerId { get; set; }
    
    
    public Data Data { get; set; }
    public Peer Peer { get; set; }
}