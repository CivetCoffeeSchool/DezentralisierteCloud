using Model.Entities;

namespace Domain.Services;

public interface IFileService
{
    Task<(int,int)> PartToSave(string fileName);

    Task<List<Peer>> GetRequiredPeerAmount(long fileSize);
    Task<int> GetHighestSerialNumber(int dataId);
}