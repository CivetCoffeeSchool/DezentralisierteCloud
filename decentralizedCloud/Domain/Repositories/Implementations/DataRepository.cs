using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class DataRepository: ARepository<Data>, IDataRepository
{
    private readonly IDataRepository _dataRepository;
    public DataRepository(DbContext context, DataRepository dataRepository) : base(context)
    {
        _dataRepository = dataRepository;
    }
    
    public async Task<List<Peer>> GetPeersByDataIdAsync(int dataId)=>
        await _dbSet.Include(d=).Where(d=>d.Id==dataId).ToListAsync();
}