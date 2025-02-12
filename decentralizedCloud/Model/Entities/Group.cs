using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("GROUPS")]
public class Group
{
    [Column("GROUP_ID"),Key]
    public int GroupId { get; set; }

    [Column("GROUP_NAME"),Required,StringLength(50)]
    public string GroupName { get; set; }
}