﻿using Model.Entities;

namespace Domain.Repositories.Interfaces;

public interface IDataRepository:IRepository<Data>
{
    Task<List<Peer>> GetPeersByDataIdAsync(int dataId);

    Task<List<Data?>> GetFilesPerFilenameAsync(string filename);

    Task<List<int>> GetSerialNumbersAsync(int dataId);
    Task<Data?> GetDataByIdAsync(int dataId);

    Task<Data?> GetFilePerFilenameAsync(string filename);
    
    Task<bool> FileExistsAsync(int dataId);
    Task<bool> FileExistsAsync(string filename);

    Task<bool> FileHashExsistsAsync(string fileHash);

    Task<string?> FilenameByFileHash(string fileHash);

    Task<Data?> GetDataByFileHash(string hash);

    Task<int?> FileIdByFileHash(string fileHash);

}