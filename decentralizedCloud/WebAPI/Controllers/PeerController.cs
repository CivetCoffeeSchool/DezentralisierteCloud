using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Entities;

namespace WebAPI.Controllers;


public class PeerController : ControllerBase
{
    private readonly IPeerRepository _peerRepository;

    public PeerController(IPeerRepository peerRepository)
    {
        _peerRepository = peerRepository;
    }


    

    /*[HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(Peer peer)
    {
        if (!ModelState.IsValid)
            return View(peer);

        _context.Peers.Add(peer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = peer.PeerId });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var peer = await _context.Peers.FindAsync(id);
        if (peer == null)
            return NotFound();

        return View(peer);
    }*/
}
