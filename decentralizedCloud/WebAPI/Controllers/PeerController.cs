using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace WebAPI.Controllers;


/*public class PeerController : Controller
{
    private readonly AppDbContext _context;

    public PeerController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
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
    }
}*/
