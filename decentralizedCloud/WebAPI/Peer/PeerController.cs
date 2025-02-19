using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Peer;
[ApiController]
[Route("api/peers")]
public class PeerController : ControllerBase
{
    private readonly IPeerRepository _peerRepo;
    private readonly IConfiguration _config;

    public PeerController(
        IPeerRepository peerRepo,
        IConfiguration config)
    {
        _peerRepo = peerRepo;
        _config = config;
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinNetwork([FromBody] PeerJoinRequest request)
    {
        // Read from file NOT database
        var networkKey = _config["Network:Key"]; 
        
        if (request.NetworkKey != networkKey)
            return Unauthorized("Invalid network key");

        var peer = new Model.Entities.Peer {
            IpAddress = request.IP,
            Port = request.Port,
            AvaliableSpace = request.AvailableSpace,
            LastHeartbeat = DateTimeOffset.UtcNow
        };

        await _peerRepo.CreateAsync(peer);
        
        return Ok(new PeerJoinResult {
            PeerId = peer.PeerId,
            NetworkId = _config["Network:Id"]
        });
    }
}
