using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class VideosController : Controller
{
    private readonly ILogger<VideosController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public VideosController(ILogger<VideosController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var videos = _context.Videos
        .Include(v => v.Modulo)
        .ThenInclude(m => m.Curso)
        .OrderBy(v => v.Modulo.Curso.Nome)
        .ToList();

        return View(videos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var video = await _context.Videos.SingleOrDefaultAsync(v => v.Id == id);
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["ModuloId"] = new SelectList(_context.Modulos, "Id", "Nome");
        return View(video);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var videos = _context.Videos
            .Include(v => v.Modulo)
            .ThenInclude(m => m.Curso)
            .OrderBy(v => v.Modulo.Curso.Nome)
            .ToList();

        var modulos = _context.Modulos
            .Include(m => m.Curso)
            .OrderBy(m => m.Curso.Nome)
            .ToList();
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["ModuloId"] = new SelectList(_context.Modulos, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Video video)
    {
        if (ModelState.IsValid)
        {
            _context.Add(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(video);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var videos = _context.Videos
            .Include(v => v.Modulo)
            .ThenInclude(m => m.Curso)
            .OrderBy(v => v.Modulo.Curso.Nome)
            .ToList();
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["ModuloId"] = new SelectList(_context.Modulos, "Id", "Nome");
        if (_context.Videos == null)
        {
            return NotFound();
        }
        var video = await _context.Videos.SingleOrDefaultAsync(m => m.Id == id);
        return View(video);
    }

    public async Task<IActionResult> EditConfirmed(int id, Video video)
    {
        if (id != video.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(video);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["ModuloId"] = new SelectList(_context.Modulos, "Id", "Nome");
        var video = await _context.Videos.SingleOrDefaultAsync(m => m.Id == id);
        if (video == null)
        {
            return NotFound();
        }
        return View(video);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var video = await _context.Videos.SingleOrDefaultAsync(v => v.Id == id);
        if (video == null)
            return NotFound();

        _context.Remove(video);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"O vídeo '{video.Titulo}' foi excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
