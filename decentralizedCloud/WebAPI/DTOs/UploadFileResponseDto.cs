using Model.Entities;

namespace WebAPI.DTOs;

public class UploadFileResponseDto
{
    // TODO Change type to Dictionary<string,List<int>> if 2 Clients have the same IP with different port
    public Dictionary<string,int> Adresses { get; set; } = new Dictionary<string, int>();
    public int SequenzNumber { get; set; } = 8;
    public int maxSequenz { get; set; } = 0;

}