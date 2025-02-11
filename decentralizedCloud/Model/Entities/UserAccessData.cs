using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("USERS_HAS_ACCESS_TO_DATA_JT")]
public class UserAccessData
{
    [Column("USER_NAME"),Required,StringLength(50)]
    public string Username { get; set; }
    public User User { get; set; }
    
    public Data Data { get; set; }
    
    [Column("DATA_ID")]
    public int DataId { get; set; }
    
    [Column("OWNERSHIP_TYPE")]
    public string ownerShipType { get; set; }
}