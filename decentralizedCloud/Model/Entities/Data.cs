using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("DATA")]
public class Data
{
    public Data()
    {
        uploadTime = DateTimeOffset.UtcNow;
    }
    
    [Column("DATA_ID")]
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("NAME"), Required, StringLength(50)]
    public string Name { get; set; }
    
    [Column("SIZE")]
    [Required]
    public int Size { get; set; }//Byte
    
    [Column("UPLOAD_TIME"),Required] 
    public DateTimeOffset uploadTime { get; init; }
    
    public List<DataOnPeers> DataDistributions { get; set; } = new List<DataOnPeers>();
}