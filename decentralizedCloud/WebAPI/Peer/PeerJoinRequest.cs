namespace WebAPI.Peer;

public class PeerJoinRequest
{
    public string NetworkKey { get; set; }
    public string IP { get; init; }
    public int Port {get; init; }
    public long AvailableSpace { get; init; }
}
    