using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("GROUPS_TABLE")]
public class Group
{
    [Column("GROUP_ID"),Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }

    [Column("GROUP_NAME"),Required,StringLength(50)]
    public string GroupName { get; set; }

    public List<GroupData> GroupDatas { get; set; }
}