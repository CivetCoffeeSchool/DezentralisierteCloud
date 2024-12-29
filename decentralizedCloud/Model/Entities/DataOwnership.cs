using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;
[Table("USERS_HAS_ACCESS_TO_DATA_JT")]
public class DataOwnership
{
    public User User { get; set; }
    [Column("USERNAME")]
    public string Username { get; set; }
    
    public Data Data { get; set; }
    [Column("DATA_ID")]
    public int DataId { get; set; }
    
    [Column("IS_OWNER")]
    public bool IsOwner { get; set; }
    //true for owner, has right to download and remove the file, also can modificate access for others
    //false for viewer, only has right to download
}