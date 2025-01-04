using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class PeerRepository: ARepository<Peer>, IPeerRepository 
{
    private readonly IPeerRepository _peerRepository;
    public PeerRepository(DbContext context, PeerRepository peerRepository) : base(context)
    {
        _peerRepository = peerRepository;
    }
}