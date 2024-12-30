using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories.Implementations;

public class DataOwnershipRepository:ARepository<DataOwnership>
{
    public DataOwnershipRepository(DbContext context) : base(context)
    {
    }
    public async Task<List<DataOwnership>> GetByUsernameAsync(string username)
    {
        return await _dbSet.Where(o => o.Username == username).Include(o => o.Data).ToListAsync();
    }

    public async Task<DataOwnership?> GetOwnershipAsync(string username, int dataId)
    {
        return await _dbSet.FirstOrDefaultAsync(o => o.Username == username && o.DataId == dataId);
    }
}