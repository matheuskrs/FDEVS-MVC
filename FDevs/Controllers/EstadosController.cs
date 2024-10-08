using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class EstadosController : Controller
{
    private readonly ILogger<EstadosController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public EstadosController(ILogger<EstadosController> logger, AppDbContext context, IWebHostEnvironment host)
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
        var estados = _context.Estados.ToList();
        return View(estados);
    }

    public async Task<IActionResult> Details(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var estado = await _context.Estados.SingleOrDefaultAsync(e => e.Id == id);
        return View(estado);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Estado estado)
    {
        if (ModelState.IsValid)
        {
            _context.Add(estado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(e => e.Errors);
        }
        return View(estado);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        if (_context.Estados == null)
        {
            return NotFound();
        }
        var estado = await _context.Estados.SingleOrDefaultAsync(e => e.Id == id);
        return View(estado);
    }

    public async Task<IActionResult> EditConfirmed(int id, Estado estado)
    {
        if (id != estado.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(estado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(estado);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var estado = await _context.Estados.SingleOrDefaultAsync(e => e.Id == id);
        if (estado == null)
        {
            return NotFound();
        }
        return View(estado);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var estado = await _context.Estados.SingleOrDefaultAsync(e => e.Id == id);
        if (estado == null)
            return NotFound();

        _context.Remove(estado);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"O estado '{estado.Nome}' foi excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
