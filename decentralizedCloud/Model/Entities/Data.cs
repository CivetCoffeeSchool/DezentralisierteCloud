using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("DATA")]
public class Data
{
    [Column("DATA_ID")]
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column("NAME")]
    [Required]
    public string Name { get; set; }
    [Column("SIZE")]
    [Required]
    public int Size { get; set; }//Byte
    
    public List<DataDistribution> DataDistributions { get; set; } = new List<DataDistribution>();
}