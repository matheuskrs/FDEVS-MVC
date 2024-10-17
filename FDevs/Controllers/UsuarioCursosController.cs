using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuarioCursosController : Controller
{
    private readonly ILogger<UsuarioCursosController> _logger;
    private readonly AppDbContext _context;

    public UsuarioCursosController(ILogger<UsuarioCursosController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var usuarioCursos = await _context.UsuarioCursos
            .Include(uc => uc.Usuario)
            .Include(uc => uc.Curso)
            .ToListAsync();
        return View(usuarioCursos);
    }

    public async Task<IActionResult> Details(string usuarioId, int cursoId)
    {
        ViewData["CursoId"] = new SelectList(_context.Cursos.ToList(), "Id", "Nome");
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios.ToList(), "UsuarioId", "Nome");
        var usuarioCurso = await _context.UsuarioCursos
            .Include(uc => uc.Usuario)
            .Include(uc => uc.Curso)
            .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CursoId == cursoId);
        if (usuarioCurso == null)
            return NotFound();

        return View(usuarioCurso);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["CursoId"] = new SelectList(_context.Cursos.ToList(), "Id", "Nome");
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios.ToList(), "UsuarioId", "Nome");
        ViewBag.Usuarios = await _context.Usuarios.ToListAsync();
        ViewBag.Cursos = await _context.Cursos.ToListAsync();
        return View();
    }

    // Cria um novo relacionamento
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioCurso usuarioCurso)
    {
        if (ModelState.IsValid)
        {
            _context.Add(usuarioCurso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuarioCurso);
    }

    public async Task<IActionResult> Edit(string usuarioId, int cursoId)
    {
        ViewData["CursoId"] = new SelectList(_context.Cursos.ToList(), "Id", "Nome");
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios.ToList(), "UsuarioId", "Nome");
        var usuarioCurso = await _context.UsuarioCursos
            .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CursoId == cursoId);
        if (usuarioCurso == null)
            return NotFound();
        return View(usuarioCurso);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditConfirmed(string usuarioId, int cursoId, UsuarioCurso usuarioCurso)
    {
        if (usuarioId != usuarioCurso.UsuarioId || cursoId != usuarioCurso.CursoId)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(usuarioCurso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(usuarioCurso);
    }

    public async Task<IActionResult> Delete(string usuarioId, int cursoId)
    {
        ViewData["CursoId"] = new SelectList(_context.Cursos.ToList(), "Id", "Nome");
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios.ToList(), "UsuarioId", "Nome");
        var usuarioCurso = await _context.UsuarioCursos
            .Include(uc => uc.Usuario)
            .Include(uc => uc.Curso)
            .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CursoId == cursoId);
        if (usuarioCurso == null)
            return NotFound();

        return View(usuarioCurso);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(string usuarioId, int cursoId)
    {
        var usuarioCurso = await _context.UsuarioCursos
            .Include(uc => uc.Usuario)
            .Include(uc => uc.Curso)
            .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CursoId == cursoId);
        if (usuarioCurso == null)
            return NotFound();

        _context.Remove(usuarioCurso);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"O relacionamento entre '{usuarioCurso.Usuario.Nome}' e o curso '{usuarioCurso.Curso.Nome}' foi excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
