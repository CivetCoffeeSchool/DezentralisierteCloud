using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("USERS")]
public class User
{
    [Column("USER_ID"),Key]
    public int UserId { get; set; }
    
    [Column("USERNAME"),StringLength(50),Required]
    public string Username{ get; set; }
    
    [Column("USER_TYPE"),StringLength(50),]
    [Required]
    public string userType{ get; set; }
    
    [Column("PASSWORD_HASH")]
    [Required]
    public string PasswordHash{ get; set; }
    
    [Column("PASSWORD_SALT")]
    [Required]
    public string PasswordSalt{ get; set; }

    public List<UserAccessData> DataOwnerships{ get; set; } = new List<UserAccessData>();
}