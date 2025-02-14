namespace WebAPI;
using System;
using System.IO;
using System.Security.Cryptography;

public class NetworkConfiguration
{
    private const string HashFilePath = "/app/volume/network-hash.txt";

    public string NetworkHash
    {
        get => LoadHashFromFile();
        set => SaveHashToFile(value);
    }

    public NetworkConfiguration()
    {
        if (string.IsNullOrEmpty(NetworkHash))
        {
            GenerateAndStoreHash();
        }
    }

    private void GenerateAndStoreHash()
    {
        using var rng = RandomNumberGenerator.Create();
        var networkHash = new byte[16];
        rng.GetBytes(networkHash);
        NetworkHash = BitConverter.ToString(networkHash).Replace("-", "");
    }

    private string LoadHashFromFile()
    {
        return File.Exists(HashFilePath) ? File.ReadAllText(HashFilePath) : string.Empty;
    }

    private void SaveHashToFile(string hash)
    {
        if (!Directory.Exists(Path.GetDirectoryName(HashFilePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(HashFilePath));
        }
        File.WriteAllText(HashFilePath, hash);
    }
}
