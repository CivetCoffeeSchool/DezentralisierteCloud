using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class DataRepository: ARepository<Data>, IDataRepository
{
    public DataRepository(NetworkinfoDbContext context) : base(context)
    {

    }
    
    public async Task<List<Peer>> GetPeersByDataIdAsync(int dataId)=>
        await _dbSet.Where(d=> d.Id==dataId).SelectMany(d=> d.DataDistributions).Select(d=>d.Peer).ToListAsync();
    
    public async Task<List<Data?>> GetFilesPerFilename(string filename) => await _dbSet.Where(d=>d.Name==filename).ToListAsync();
}