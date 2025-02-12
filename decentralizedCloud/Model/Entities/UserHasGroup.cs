using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("USER_HAS_USER_GROUP_JT")]
public class UserHasGroup
{
    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("GROUP_ID")]
    public int GroupId { get; set; }

    public User User { get; set; }
    public Group Group { get; set; }
}