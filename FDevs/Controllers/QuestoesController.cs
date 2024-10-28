using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class QuestoesController : Controller
{
    private readonly ILogger<QuestoesController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public QuestoesController(ILogger<QuestoesController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;

    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["ProvaId"] = new SelectList(_context.Provas, "Id", "Nome");
        var questoes = await _context.Questoes.Include(q => q.Prova).ToListAsync();
        return View(questoes);
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewData["ProvaId"] = new SelectList(_context.Provas, "Id", "Nome");
        var questao = await _context.Questoes.SingleOrDefaultAsync(q => q.Id == id);
        return View(questao);
    }

    [HttpGet]
    public IActionResult Create()
    {

        ViewData["ProvaId"] = new SelectList(_context.Provas, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Questao questao)
    {

        if (ModelState.IsValid)
        {
            _context.Add(questao);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"A questão {questao.Texto} foi criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(q => q.Errors);
        }
        return View(questao);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["ProvaId"] = new SelectList(_context.Provas, "Id", "Nome");
        if (_context.Questoes == null)
        {
            return NotFound();
        }
        var questao = await _context.Questoes.SingleOrDefaultAsync(q => q.Id == id);
        return View(questao);
    }

    public async Task<IActionResult> EditConfirmed(int id, Questao questao)
    {
        if (id != questao.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(questao);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"A questão foi alterada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return View(questao);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["ProvaId"] = new SelectList(_context.Cursos, "Id", "Nome");
        var questao = await _context.Questoes.SingleOrDefaultAsync(q => q.Id == id);
        if (questao == null)
        {
            return NotFound();
        }
        return View(questao);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var questao = await _context.Questoes
            .Include(q => q.Alternativas)
            .SingleOrDefaultAsync(q => q.Id == id);
        if (questao == null)
            return NotFound();

        var permitirExcluir = true;

        var alternativasDaQuestao = questao.Alternativas.Any();

        if (alternativasDaQuestao)
            permitirExcluir = false;

        if (!permitirExcluir)
        {
            TempData["Warning"] = $"A questão não pode ser excluída pois já existem registros na tabela: \"{(alternativasDaQuestao ? "ALTERNATIVAS" : "")}\"associados a ele!";
            return RedirectToAction(nameof(Index));
        }

        _context.Remove(questao);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"A questão '{questao.Texto}' foi excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
