using Model.Entities;

namespace WebAPI.DTOs;

public class UploadFileResponseDto
{
    public Model.Entities.Peer FullCopyPeer { get; set; }
    public Model.Entities.Peer Part1Peer { get; set; }
    public Model.Entities.Peer Part2Peer { get; set; }
}