using System.Net;
using Domain.Repositories.DTOs;
using Domain.Repositories.Interfaces;
using Domain.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/files")]
public class FileController: ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPeerRepository _peerRepository;
    private readonly IDataRepository _dataRepository;
    private readonly IRepository<DataOnPeers> _dataOnPeersRepository;
    private readonly IFileService _fileService;
    public FileController(IUserRepository userRepository, IPeerRepository peerRepository, IDataRepository dataRepository, IRepository<DataOnPeers> dataOnPeersRepository, IFileService fileService)
    {
        _userRepository = userRepository;
        _peerRepository = peerRepository;
        _dataRepository = dataRepository;
        _dataOnPeersRepository = dataOnPeersRepository;
        _fileService = fileService;
    }

    #region Download
    
    // TODO Filehash für jedes File erstellen

    
    #region GetIPAddresses
    
    // Returns when files
    [HttpGet("GetPeersByFilename")]
    public async Task<IActionResult> GetPeers([FromQuery] string filename)
    {
        // Mit Filename die Peers bekommen
        // derzeit nicht verwendet da mit ID funktioniert
        Console.WriteLine($"Calling GetIpForData with filename: {filename}");
        
        // TODO negieren
        if (await _dataRepository.FileExistsAsync(filename))
        {
            Console.WriteLine("File found");
            Dictionary<string, int> ips = new Dictionary<string, int>();
            int dataId = await FilenameToDataId(filename);
            List<Model.Entities.Peer> peers = await _dataRepository.GetPeersByDataIdAsync(dataId);
            if (peers.Count == 0)
            {
                Console.WriteLine("No peer found");
                return NotFound("File not available");
            }
            foreach (var p in peers)
            {
                ips.Add(p.IpAddress, p.Port);
            }
            Console.WriteLine($"Found {peers.Count} peers");
            return Ok(ips);
            
            /* With not unique filenames
            List<Peer> peers = new List<Peer>();
            Dictionary<string,int> peerIds = new Dictionary<string, int>();
            foreach (var file in await _dataRepository.GetFilesPerFilenameAsync(filename))
            {
                Console.WriteLine($"FileId: {file.Id}");
               peers = _dataRepository.GetPeersByDataIdAsync(file.Id).GetAwaiter().GetResult();
               foreach (var p in peers)
               {
                   peerIds[p.IpAddress]=p.Port;
               }
            }
            return Ok(peerIds);*/
        }
        else
        {
            Console.WriteLine($"File {filename} not found");
            return NotFound("File not found");
        }
        
        // Aufteilen: Sequenznumber_Filename

        return NotFound();
    }

    [HttpGet("GetPeers")]
    public async Task<IActionResult> GetPeers(int dataId)
    {
        // Getting the IP-Addresses and Ports of the Peers which hold the given File
        // TODO check if Peers are available 
        // TODO When one Part is on multiple Peers only return 1 per Part
        Console.WriteLine($"Calling GetPeers with id: {dataId}");
        Dictionary<string, int> ips = new Dictionary<string, int>();
        List<Model.Entities.Peer> peers = await _dataRepository.GetPeersByDataIdAsync(dataId);
        if (peers.Count == 0)
        {
            Console.WriteLine("No peer found");
            return NotFound("File not available");
        }
        foreach (var p in peers)
        {
            ips.Add(p.IpAddress, p.Port);
        }
        Console.WriteLine($"Found {peers.Count} peers");
        return Ok(ips);
    }
    
    #endregion
    
    
    [HttpGet("PartToSave")]
    public async Task<IActionResult> PartToSaveOnPeer([FromQuery] string fileName)
    {
        // Get Filepart least occurring in Network
        // Sending Start and Endbyte
        int fileId = await FilenameToDataId(fileName);
        Console.WriteLine($"Calling PartToSaveOnPeer with id: {fileId}");
        Data? data = await _dataRepository.GetDataByIdAsync(fileId);
        if (data == null)
        {
            Console.WriteLine($"File {fileId} not found");
            return NotFound("File not found");
        }

        string ipAddress = "";
        int port = 0;
        (ipAddress, port) = GetIpFromRequest(HttpContext.Request);

        if (!await _peerRepository.GetPeerSavesFile(ipAddress, port, data.Id))
        {
            return Ok();
        }
        
        return Ok(_fileService.PartToSave(fileName));
    }

    [HttpPost("PeerSavedFile")]
    public async Task<IActionResult> SavePeerSavedFileOnPeer([FromQuery] string fileName, [FromQuery] int sequenznumber)
    {
        string ipAddress = "";
        int port = 0;
        (ipAddress, port) = GetIpFromRequest(HttpContext.Request);
        Model.Entities.Peer peer = await _peerRepository.FindPeerByIpAndPortAsync(ipAddress, port);
        Data? data = await _dataRepository.GetFilePerFilenameAsync(fileName);
        if (peer == null)
        {
            return NotFound($"Peer on {ipAddress}:{port} not found");
        }
        else if (data == null)
        {
            return NotFound($"File {fileName} not found");
        }

        _dataOnPeersRepository.CreateAsync(new DataOnPeers()
        {
            Data = data, DataId = data.Id, Peer = peer, PeerId = peer.PeerId, SequenceNumber = sequenznumber
        });
        return Ok();
    }
    
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<int> FilenameToDataId(string filename)
    {
        Data? d = await _dataRepository.GetFilePerFilenameAsync(filename);
        return d.Id;
    }
    
    [ApiExplorerSettings(IgnoreApi = true)]
    private (string,int) GetIpFromRequest(HttpRequest request)
    {
        string? ipaddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        int port = HttpContext.Connection.RemotePort;
        return (ipaddress, port);
    }
    
    
    // Not unique filenames
    [HttpGet("FileInfoPerFilename")]
    private async Task<IActionResult> GetFileInfoPerFilename([FromQuery] string filename)
    {
        // Getting each File with the given filename
        // Wenn filenames nicht unique
        Console.WriteLine($"Calling GetFileInfoPerFilename with filename: {filename}");
        List<Data?> dataFiles = await _dataRepository.GetFilesPerFilenameAsync(filename);
        if (dataFiles.Count > 0)
        {
            return Ok(dataFiles);
        }
        return NotFound("No file found");
    }
    
    #endregion

    
    // Steps
    
    // 1. Send Fileinformation to Server Check
    // 2. Check if File already uploaded Check
    
    // 3a File existiert Server sendet zurück welchen Teil Peer Speichern soll
    // 4a File wird an Peer geschickt
    // 5a Bestätigung am Server
    
    // 3b File existiert nicht. Server checkt Filegröße und die erreichbaren Peers
    
    // 4ba ein von beiden zu gering File wird als ganzes gespeichert
    
    // 4bb File wird aufgeteilt und Teilgrößen und beginne werden zurückgesendet
    // 5b Fileteile werden an Peers gesendet
    
    // 6 Peers Confirm Saving
    // 7 Update Database

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile([FromBody] UploadRequestDto fileInfo)
    {
        // Returns 
        // TODO Database entry's 
        
        // Serialnumber 8 is Errorcode
        Console.WriteLine("Calling UploadFile");
        Console.WriteLine($"Filename {fileInfo.FileName}, FileSize {fileInfo.FileSize}, FileHash {fileInfo.FileHash}");
        UploadFileResponseDto responseDto = new UploadFileResponseDto();
        if (fileInfo.FileName.Length == 0 || fileInfo.FileSize == 0 || fileInfo.FileHash.Length == 0)
        {
            return NotFound(responseDto);
        }


        
        if(await _dataRepository.FileHashExsistsAsync(fileInfo.FileHash))
        {
            // Return (Serialnumber to Save, Hightest SerialNumber)
            (responseDto.SequenzNumber, responseDto.maxSequenz) =  await _fileService.PartToSave(fileInfo.FileHash);
            return Conflict(responseDto);
        }
        else
        {
            await _dataRepository.CreateAsync(new Data()
            {
                Name = fileInfo.FileName,
                FileHash = fileInfo.FileHash,
                Size = fileInfo.FileSize,
                UploadTime = DateTime.Now,
                UploaderId = 1
            });
        }
        List<Model.Entities.Peer> peers = await _fileService.GetRequiredPeerAmount(fileInfo.FileSize);
        responseDto.SequenzNumber = 9;
        responseDto.maxSequenz = 9;
        if (peers.Count == 0)
        {
            return Ok(responseDto);
        }
        
        // Finden der höchsten SerialNumber 
        responseDto.maxSequenz = peers.Count()-1;
        if (responseDto.maxSequenz == 0)
        {
            responseDto.maxSequenz = 9;
        }
        Console.WriteLine($"Highest serial number: {responseDto.maxSequenz}");
        
        responseDto.Adresses = peers.ToDictionary(peer => peer.IpAddress, peer => peer.Port);
        return Ok(responseDto);
    }

    [HttpGet("ConfirmDownload")]
    public async Task<IActionResult> ConfirmDownload([FromQuery] string fileName, [FromQuery] int serialNumber, [FromQuery] string chunckHash)
    {
        string ipAddress = "10.232.33.171";
        int port =5192;
        Model.Entities.Peer? peer = await _peerRepository.FindPeerByIpAndPortAsync(ipAddress, port);
        Data? data = await _dataRepository.GetFilePerFilenameAsync(fileName);
        if (data == null)
        {
            return NotFound($"File {fileName} not found");
        }

        if (peer == null)
        {
            return NotFound($"Peer on {ipAddress}:{port} not found");
        }

        if (await _dataOnPeersRepository.ExistsAsync(dp => dp.DataId == data.Id))
        {
            if (!await _dataOnPeersRepository.ExistsAsync(dp => dp.ChunkHash == chunckHash))
            {
                return BadRequest($"Filehash has changed");
            }
        };
        DataOnPeers dataOnPeers =new DataOnPeers()
        {
            DataId = data.Id, 
            PeerId = peer.PeerId, 
            SequenceNumber = serialNumber,
            ChunkSize = data.Size, 
            ChunkHash = chunckHash
        };
        
        await  _dataOnPeersRepository.CreateAsync(dataOnPeers);
        return Ok();
    }
    
    /*[HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequestDto request)
    {
        // Step 1: Authenticate the user
        var user = await _userRepository.AuthenticateUserAsync(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        // Step 2: Register the file in the database
        var data = new Data
        {
            Name = request.FileName,
            Size = request.FileSize
        };
        data = await _dataRepository.CreateAsync(data);

        //Assign peers for the data
        var assignedPeers = await _peerService.AssignPeersForDataAsync(request.FileSize);

        // Notify the source peer to send data to others
        var splits = new List<(Peer destination, int dataSize)>
        {
            (assignedPeers[0], request.FileSize / 2),
            (assignedPeers[1], request.FileSize / 2)
        };
        var sourcePeer = await _peerRepository.FindPeerByIpAndPortAsync(HttpContext.Connection.RemoteIpAddress.ToString(), HttpContext.Connection.RemotePort);
        if (sourcePeer == null)
        {
            return BadRequest("Source peer is not recognized in the network.");
        }
        await _peerService.NotifyPeersToReceiveDataAsync(sourcePeer, splits);

        return Ok("File uploaded and distributed successfully.");
    }
    
    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] DownloadFileRequestDto request)
    {
        // Step 1: Authenticate the user
        var user = await _userRepository.AuthenticateUserAsync(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }
        
        var tmpuser = await _userRepository.ReadGraphAsync(request.Username);
        var dataOwnerships = tmpuser?.DataOwnerships;
        var datas = dataOwnerships.Select(d => d.Data).ToList();
        if (!datas.Any(d => d.Id == request.Id))
        {
            return Unauthorized("User does not have access to this file.");
        }

        // Step 2: Find the requested file in the database
        var data = await _dataRepository.ReadAsync(d => d.Id == request.Id);
        if (data == null || !data.Any())
        {
            return NotFound("File not found.");
        }

        // Step 3: Find peers that store the file
        var dataEntity = data.First();
        var peersWithFile = await _peerRepository.GetPeersByDataIdAsync(dataEntity.Id);
        if (peersWithFile == null || !peersWithFile.Any())
        {
            return NotFound("No peers currently have this file.");
        }

        // Step 4: Return peer information to the client
        var peerLocations = peersWithFile.Select(peer => new
        {
            PeerId = peer.Id,
            IpAddress = peer.IpAddress,
            Port = peer.Port
        }).ToList();

        return Ok(new
        {
            FileName = request.FileName,
            Peers = peerLocations
        });
    }*/
}