using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;
[Authorize(Roles = "Administrador")]
public class AlternativasController : Controller
{
    private readonly ILogger<AlternativasController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public AlternativasController(ILogger<AlternativasController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
        var alternativas = await _context.Alternativas
            .Include(a => a.Questao)
            .ThenInclude(q => q.Prova)
            .ToListAsync();
        return View(alternativas);
    }

    public async Task<IActionResult> Details(int id)
    {

        ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
        var alternativa = await _context.Alternativas.SingleOrDefaultAsync(q => q.Id == id);
        return View(alternativa);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Alternativa alternativa)
    {
        if (ModelState.IsValid)
        {
            _context.Add(alternativa);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"A alternativa {alternativa.Texto} foi criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(a => a.Errors);
        }
        return View(alternativa);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
        if (_context.Alternativas == null)
        {
            return NotFound();
        }
        var alternativa = await _context.Alternativas.SingleOrDefaultAsync(a => a.Id == id);
        return View(alternativa);
    }

    public async Task<IActionResult> EditConfirmed(int id, Alternativa alternativa)
    {
        if (id != alternativa.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(alternativa);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"A alternativa foi alterada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return View(alternativa);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
        var alternativa = await _context.Alternativas.SingleOrDefaultAsync(a => a.Id == id);
        if (alternativa == null)
        {
            return NotFound();
        }
        return View(alternativa);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var alternativa = await _context.Alternativas.SingleOrDefaultAsync(a => a.Id == id);
        if (alternativa == null)
            return NotFound();

        var respostas = await _context.Respostas.AnyAsync(r => r.AlternativaId == id);

        if (respostas)
        {
            TempData["Warning"] = $"A alternativa não pode ser excluída pois já existem registros na tabela: \"RESPOSTAS\" associados a ela!";
            return RedirectToAction(nameof(Index));
        }

        _context.Remove(alternativa);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"A alternativa '{alternativa.Texto}' foi excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
