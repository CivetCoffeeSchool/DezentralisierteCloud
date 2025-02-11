using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("USERS")]
public class User
{
    [Column("USERNAME_ST")]
    [Key]
    public string Username{ get; set; }
    
    [Column("USER_TYPE"),MaxLength(50)]
    [Required]
    public string userType{ get; set; }
    
    [Column("PASSWORD_HASH")]
    [Required]
    public string PasswordHash{ get; set; }
    
    [Column("PASSWORD_SALT")]
    [Required]
    public string PasswordSalt{ get; set; }

    public List<DataOwnership> DataOwnerships{ get; set; } = new List<DataOwnership>();
}