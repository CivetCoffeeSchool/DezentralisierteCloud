// using Domain.Repositories.Interfaces;
// using Microsoft.EntityFrameworkCore;
// using Model.Configuration;
// using Model.Entities;
//
// namespace Domain.Repositories.Implementations;
//
// public class GroupRepository : ARepository<Group>, IGroupRepository
// {
//     public GroupRepository(NetworkinfoDbContext context) : base(context)
//     {
//     }
//     
//     public async Task<string?> GetOwnerShipType(int groupId, int dataId) => await _dbSet
//         .Where(g => g.GroupId == groupId)
//         .Include(g => g.GroupDatas)
//         .Select(g => g.GroupDatas.Where(gd => gd.DataId == dataId).Select(gd => gd.ownershipType).FirstOrDefault()).FirstOrDefaultAsync();
//         
// }