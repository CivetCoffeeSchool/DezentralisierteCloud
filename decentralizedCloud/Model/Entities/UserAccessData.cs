using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("USERS_HAS_ACCESS_TO_DATA_JT_ST")]
public class UserAccessData
{
    [Column("USER_ID")]
    public int UserId { get; set; }
    
    [Column("DATA_ID")]
    public int DataId { get; set; }
    
    [Column("OWNERSHIP_TYPE")]
    public string OwnershipType { get; set; }
    
    public Data Data { get; set; }
    public User User { get; set; }
}