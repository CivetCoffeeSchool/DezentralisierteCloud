using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("PEERS_ST")]
public class Peer
{
    [Column("ID")]
    [Key]
    public string MacAddress { get; set; }
    [Column("PEER_TYPE"),MaxLength(50)]
    [Required]
    public string peerType { get; set; }
    [Column("TOTAL_SPACE")]
    [Required]
    public int TotalSpace { get; set; }
    [Column("IP_ADDRESS")]
    [Required]
    public string IpAddress { get; set; }
    [Column("PORT")]
    [Required]
    public int Port { get; set; }

}