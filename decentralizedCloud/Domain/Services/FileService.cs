using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Model.Entities;

namespace Domain.Services;

public class FileService : IFileService
{
    public readonly IPeerRepository _peerRepository;
    public readonly IDataRepository _dataRepository;
    
    public FileService(IPeerRepository peerRepository, IDataRepository dataRepository)
    {
        _peerRepository = peerRepository;
        _dataRepository = dataRepository;
    }

    public async Task<(int,int)> PartToSave(string fileHash)
    {
        if (!await _dataRepository.FileHashExsistsAsync(fileHash))
        {
            return (8,0);
        }
        Data data = await _dataRepository.GetDataByFileHash(fileHash) ?? new Data();
        List<int> serialNumbers = await _dataRepository.GetSerialNumbersAsync(data.Id);
        if (serialNumbers.Count==0)
        {
            return (9,9);
        }

        var max = serialNumbers.Where(s => s != 9);
        if (!max.Any())
        {
            // TODO Überprüfung ob File gespalten werden kann falls noch nicht ist
            return (9, 9);
        }
        return (GetLeastUsedFilepart(serialNumbers), max.Max());
    }

    private int GetLeastUsedFilepart(List<int> serialNumbers)
    {
        if (serialNumbers == null || serialNumbers.Count == 0)
        {
            return 8;
        }

        if (serialNumbers.Count(s => s == 9) > 1)
        {
            return 9;
        }
        int leastUsed = serialNumbers
            .GroupBy(x => x)
            .OrderBy(x => x.Count())
            .First()
            .Key;
        return leastUsed;
    }

    public async Task<int> GetHighestSerialNumber(int dataId)
    {
        List<int> serialNumbers = await _dataRepository.GetSerialNumbersAsync(dataId);
        var max = serialNumbers.Where(s => s != 9);
        if (!max.Any())
        {
            // TODO Überprüfung ob File gespalten werden kann falls noch nicht ist
            return 9;
        }
        return max.Max();
    }
    
    public async Task<List<Peer>> GetRequiredPeerAmount(long fileSize)
    {
        Console.WriteLine($"GetRequiredPeerAmount: {fileSize}");
        
        // Step 1: Determine the number of peers required
        int requiredPeerCount = fileSize switch
        {
            < 128_000 => 1,
            < 256_000 => 2,
            _ => 4
        };
        Console.WriteLine($"Amount by fileSize: {requiredPeerCount}");
        
        List<Peer> peers = await _peerRepository.ReadAllAsync();
        if (peers.Count == 0)
        {
            return new List<Peer>();
        }
        do{
        // Step 2: Filter peers with enough available space for the file size divided by the number of peers
        // TODO Heartbeat Check
        var peersWithEnoughSpace = peers
            .Where(p => p.AvaliableSpace >= fileSize / requiredPeerCount)
            .OrderByDescending(p => p.AvaliableSpace) // Prioritize peers with more available space
            .ToList();

        // Step 3: Check if the required number of peers have enough space
        
            if (peersWithEnoughSpace.Count >= requiredPeerCount)
            {
                Console.WriteLine($"Amount avaliable peers: {requiredPeerCount}");
                return peersWithEnoughSpace.Take(requiredPeerCount).ToList();
            }

            // Reduce the number of peers and try again
            requiredPeerCount--;
        }while(requiredPeerCount > 0);

        // If no peers can save the file
        return new List<Peer>();
    }
    
}