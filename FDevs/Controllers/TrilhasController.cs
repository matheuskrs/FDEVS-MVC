using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class TrilhasController : Controller
{
    private readonly ILogger<TrilhasController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public TrilhasController(ILogger<TrilhasController> logger, AppDbContext context, IWebHostEnvironment host)
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
        var trilhas = _context.Trilhas.ToList();
        return View(trilhas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var trilha = await _context.Trilhas.SingleOrDefaultAsync(t => t.Id == id);
        return View(trilha);
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
    public async Task<IActionResult> Create(Trilha trilha, IFormFile Arquivo)
    {
        if (ModelState.IsValid)
        {
            if (Arquivo != null)
            {
                string fileName = trilha.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = Path.Combine(_host.WebRootPath, "img\\Trilhas");
                string novoArquivo = Path.Combine(caminho, fileName);
                using (var stream = new FileStream(novoArquivo, FileMode.Create))
                {
                    Arquivo.CopyTo(stream);
                }
                trilha.Foto = "\\img\\Trilhas\\" + fileName;
                await _context.SaveChangesAsync();
            }
            _context.Add(trilha);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(t => t.Errors);
        }
        return View(trilha);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        if (_context.Trilhas == null)
        {
            return NotFound();
        }
        var trilha = await _context.Trilhas.SingleOrDefaultAsync(t => t.Id == id);
        return View(trilha);
    }

    public async Task<IActionResult> EditConfirmed(int id, Trilha trilha, IFormFile Arquivo)
    {
        if (id != trilha.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {

            if (Arquivo != null)
            {
                string fileName = trilha.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = Path.Combine(_host.WebRootPath, "img\\Trilhas");
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                string novoArquivo = Path.Combine(caminho, fileName);
                using (var stream = new FileStream(novoArquivo, FileMode.Create))
                {
                    Arquivo.CopyTo(stream);
                }
                trilha.Foto = "\\img\\Trilhas\\" + fileName;
            }
            _context.Update(trilha);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        return View(trilha);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var trilha = await _context.Trilhas.SingleOrDefaultAsync(t => t.Id == id);
        if (trilha == null)
        {
            return NotFound();
        }
        return View(trilha);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var trilha = await _context.Trilhas.SingleOrDefaultAsync(t => t.Id == id);
        if (trilha == null)
            return NotFound();

        _context.Remove(trilha);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"A trilha '{trilha.Nome}' foi excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
