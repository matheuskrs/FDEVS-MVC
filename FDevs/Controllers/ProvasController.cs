using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class ProvasController : Controller
{
    private readonly ILogger<ProvasController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public ProvasController(ILogger<ProvasController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var provas = await _context.Provas.Include(p => p.Curso).ToListAsync();
        return View(provas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var prova = await _context.Provas.Include(p => p.Curso).SingleOrDefaultAsync(p => p.Id == id);
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        return View(prova);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Prova prova)
    {
        if (ModelState.IsValid)
        {
            _context.Add(prova);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(p => p.Errors);
        }
        return View(prova);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        if (_context.Provas == null)
        {
            return NotFound();
        }
        var prova = await _context.Provas.SingleOrDefaultAsync(p => p.Id == id);
        return View(prova);
    }

    public async Task<IActionResult> EditConfirmed(int id, Prova prova)
    {
        if (id != prova.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(prova);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(prova);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
        var prova = await _context.Provas.SingleOrDefaultAsync(p => p.Id == id);
        if (prova == null)
        {
            return NotFound();
        }
        return View(prova);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var prova = await _context.Provas.SingleOrDefaultAsync(p => p.Id == id);
        if (prova == null)
            return NotFound();

        _context.Remove(prova);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"A Prova '{prova.Nome}' foi excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}