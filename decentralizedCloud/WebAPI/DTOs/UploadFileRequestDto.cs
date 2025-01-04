namespace WebAPI.DTOs;

public class UploadFileRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FileName { get; set; }
    public int FileSize { get; set; } // File size in bytes
}