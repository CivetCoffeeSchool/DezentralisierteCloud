using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("DATA_SAVED_ON_PEERS_JT")]
public class DataDistribution
{
    [Column("SEQUENCE_NUMBER"),Key]
    public int SequenceNumber { get; set; }
    
    public Data Data { get; set; }
    [Column("DATA_ID")]
    public int DataId { get; set; }
    
    public Peer Peer { get; set; }
    [Column("PEER_ID")]
    public string PeerMacAddress { get; set; }
}