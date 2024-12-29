using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("PEERS")]
public class Peer
{
    [Column("ID")]
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("IS_SUPERPEER")]
    [Required]
    public bool IsSuperpeer { get; set; }
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