/*using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/network")]
public class NetworkController:ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IDataRepository _dataOwnershipRepository;
    private readonly IPeerRepository _peerRepository;
    private NetworkConfiguration _networkConfiguration;
    private NetworkInitService _networkInitService;

    public NetworkController(
        IUserRepository userRepository, 
        IDataRepository dataOwnershipRepository,
        IPeerRepository peerRepository, 
        NetworkConfiguration networkConfiguration,
        NetworkInitService networkInitService)
    {
        _userRepository = userRepository;
        _dataOwnershipRepository = dataOwnershipRepository;
        _peerRepository = peerRepository;
        _networkConfiguration = networkConfiguration;
        _networkInitService = networkInitService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartNetwork(string adminUsername, string adminPassword, int totalSpace)
    {
        await _networkInitService.InitializeNetworkAsync();
        return Ok();
        //TODO: 
    }
    
}*/