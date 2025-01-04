using Model.Entities;

namespace WebAPI.DTOs;

public class UploadFileResponseDto
{
    public Peer FullCopyPeer { get; set; }
    public Peer Part1Peer { get; set; }
    public Peer Part2Peer { get; set; }
}