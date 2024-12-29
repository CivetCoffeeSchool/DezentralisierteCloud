using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("USERS")]
public class User
{
    [Column("USERNAME")]
    [Key]
    public string Username{ get; set; }//is unique
    [Column("IS_ADMIN")]
    [Required]
    public bool IsAdmin{ get; set; }
    [Column("PASSWORD_HASH")]
    [Required]
    public string PasswordHash{ get; set; }
    [Column("PASSWORD_SALT")]
    [Required]
    public string PasswordSalt{ get; set; }
}