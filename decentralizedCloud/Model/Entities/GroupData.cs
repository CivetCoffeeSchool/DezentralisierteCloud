using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("GROUP_HAS_DATA_JT_ST")]
public class GroupData
{
    [Column("GROUP_ID")]
    public int GroupId { get; set; }

    [Column("DATA_ID")]
    public int DataId { get; set; }

    [Column("OWNERSHIP_TYPE"),StringLength(50),Required]
    public string ownershipType { get; set; }

    public Group Group { get; set; }

    public Data Data { get; set; }
}