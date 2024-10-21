using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class ModulosController : Controller
{
    private readonly ILogger<ModulosController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public ModulosController(ILogger<ModulosController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var modulos = await _context.Modulos.Include(m => m.Curso).ToListAsync();
        return View(modulos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        return View(modulo);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Modulo modulo)
    {
        if (ModelState.IsValid)
        {
            var usuariosCurso = await _context.UsuarioCursos
                .Where(uc => uc.CursoId == modulo.CursoId)
                .ToListAsync();
                
            _context.Add(modulo);
            await _context.SaveChangesAsync();

            foreach (var usuarioCurso in usuariosCurso)
            {
                var usuarioEstadoModulo = new UsuarioEstadoModulo
                {
                    UsuarioId = usuarioCurso.UsuarioId,
                    ModuloId = modulo.Id,
                    EstadoId = 3
                };
                _context.Add(usuarioEstadoModulo);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modulo);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        if (_context.Modulos == null)
        {
            return NotFound();
        }
        var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
        return View(modulo);
    }

    public async Task<IActionResult> EditConfirmed(int id, Modulo modulo)
    {

        if (id != modulo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(modulo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(modulo);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
        if (modulo == null)
        {
            return NotFound();
        }
        return View(modulo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var modulo = await _context.Modulos
            .Include(m => m.Videos)
            .SingleOrDefaultAsync(m => m.Id == id);
        if (modulo == null)
            return NotFound();

        foreach (var video in modulo.Videos)
        {
            var usuariosEstadoVideo = await _context.UsuarioEstadoVideos
                .Where(uev => uev.VideoId == video.Id)
                .ToListAsync();

            foreach (var estado in usuariosEstadoVideo)
            {
                _context.Remove(estado);
            }
        }

        var usuariosEstadoModulo = await _context.UsuarioEstadoModulos
            .Where(uem => uem.ModuloId == modulo.Id)
            .ToListAsync();

        foreach (var estado in usuariosEstadoModulo)
        {
            _context.Remove(estado);
        }

        _context.Remove(modulo);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"O módulo '{modulo.Nome}' foi excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}

