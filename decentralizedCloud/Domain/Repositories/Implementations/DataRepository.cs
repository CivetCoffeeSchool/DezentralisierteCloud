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
    
    public async Task<List<Peer>> GetPeersByDataIdAsync(int dataId) =>
        await _dbSet
            .Where(d => d.Id == dataId)
            .Include(d => d.DataDistributions) 
            .ThenInclude(dd => dd.Peer)        
            .SelectMany(d => d.DataDistributions.Select(dd => dd.Peer))
            .ToListAsync();
    
    public async Task<List<Data?>> GetFilesPerFilenameAsync(string filename) => await _dbSet.Where(d=>d.Name==filename).ToListAsync();
    
    public async Task<Data?> GetFilePerFilenameAsync(string filename) => await _dbSet.Where(d => d.Name==filename).FirstOrDefaultAsync();
    public async Task<bool> FileExistsAsync(int dataId) => await _dbSet.AnyAsync(d => d.Id == dataId);
    public async Task<bool> FileExistsAsync(string filename) => await _dbSet.AnyAsync(d => d.Name == filename);

    public async Task<List<int>> GetSerialNumbersAsync(int dataId)=> await _dbSet.Where(d => d.Id == dataId).Include(d => d.DataDistributions).SelectMany(dd=> dd.DataDistributions).Select(d => d.SequenceNumber).ToListAsync();
    
    public async Task<Data?> GetDataByIdAsync(int dataId) => await _dbSet.Where(d => d.Id == dataId).FirstOrDefaultAsync();
}