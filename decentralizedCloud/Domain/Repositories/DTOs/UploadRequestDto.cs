namespace Domain.Repositories.DTOs;

public class UploadRequestDto
{
    public string FileName { get; set; }
    
    public string FileHash { get; set; }
    
    public int FileSize { get; set; }
}