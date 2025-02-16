using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Model.Entities;
[Table("USERS_ST")]
public abstract class User
{
    [Column("USER_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
    public int UserId { get; set; }
    
    [Column("USERNAME"),StringLength(50)]
    public string Username{ get; set; }
    
    [Column("USER_TYPE"),StringLength(20)]
    public string userType{ get; set; }
    
    [Column("PASSWORD_HASH")]
    public string PasswordHash{ get; set; }
    
    [Column("PASSWORD_SALT")]
    public string PasswordSalt{ get; set; }

    public List<UserAccessData> DataAccesses{ get; set; } = new List<UserAccessData>();
    public List<Data> DataOwnerships{ get; set; } = new List<Data>();
    // public List<UserHasGroup> UserGroups{ get; set; } = new List<UserHasGroup>();
}