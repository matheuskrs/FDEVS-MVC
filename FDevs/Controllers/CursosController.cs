using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using FDevs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class CursosController : Controller
{
    private readonly ILogger<CursosController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;


    public CursosController(ILogger<CursosController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cursos = await _context.Cursos
            .Include(c => c.Trilha)
            .ToListAsync();
        return View(cursos);
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
        var curso = await _context.Cursos.SingleOrDefaultAsync(c => c.Id == id);
        return View(curso);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Curso curso, IFormFile Arquivo)
    {
        if (ModelState.IsValid)
        {
            if (Arquivo != null)
            {
                string fileName = curso.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = Path.Combine(_host.WebRootPath, "img\\Cursos");
                string novoArquivo = Path.Combine(caminho, fileName);
                using (var stream = new FileStream(novoArquivo, FileMode.Create))
                {
                    Arquivo.CopyTo(stream);
                }
                curso.Foto = "\\img\\Cursos\\" + fileName;
                await _context.SaveChangesAsync();
            }
            _context.Add(curso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
        }
        return View(curso);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
        if (_context.Cursos == null)
        {
            return NotFound();
        }
        var curso = await _context.Cursos.SingleOrDefaultAsync(c => c.Id == id);
        return View(curso);
    }

    public async Task<IActionResult> EditConfirmed(int id, Curso curso, IFormFile Arquivo)
    {
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome", curso.TrilhaId);
        if (id != curso.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {

            if (Arquivo != null)
            {
                string fileName = curso.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = Path.Combine(_host.WebRootPath, "img\\Cursos");
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                string novoArquivo = Path.Combine(caminho, fileName);
                using (var stream = new FileStream(novoArquivo, FileMode.Create))
                {
                    Arquivo.CopyTo(stream);
                }
                curso.Foto = "\\img\\Cursos\\" + fileName;
            }
            _context.Update(curso);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        return View(curso);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var curso = await _context.Cursos.SingleOrDefaultAsync(c => c.Id == id);
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
        if (curso == null)
        {
            return NotFound();
        }
        return View(curso);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var curso = await _context.Cursos.SingleOrDefaultAsync(c => c.Id == id);
        if (curso == null)
            return NotFound();

        _context.Remove(curso);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"O curso '{curso.Nome}' foi excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}