using FDevs.Data;
using FDevs.Models;
using FDevs.Services.EstadoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class EstadosController : Controller
{
    private readonly ILogger<EstadosController> _logger;
    private readonly AppDbContext _context;
    private readonly IEstadoService _service;

    public EstadosController(ILogger<EstadosController> logger, AppDbContext context, IEstadoService service)
    {
        _logger = logger;
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var estados = await _service.GetEstadosAsync();
        return View(estados);
    }

    public async Task<IActionResult> Details(int id)
    {
        var estado = await _service.GetEstadoByIdAsync(id);
        return View(estado);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Estado estado)
    {
        if (ModelState.IsValid)
        {
            await _service.Create(estado);
            TempData["Success"] = $"O estado {estado.Nome} foi criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(estado);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var estado = await _service.GetEstadoByIdAsync(id);
        return View(estado);
    }

    public async Task<IActionResult> EditConfirmed(Estado estado)
    {
        if (ModelState.IsValid)
        {
            await _service.Update(estado);
            TempData["Success"] = $"O estado {estado.Nome} foi alterado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return View(estado);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var estado = await _service.GetEstadoByIdAsync(id);
        return View(estado);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var estado = await _service.GetEstadoByIdAsync(id);
        if (estado == null) return NotFound();

        var permitirExcluir = true;

        var usuarioEstadoCurso = await _context.UsuarioEstadoCursos
            .AnyAsync(uec => uec.EstadoId == id);
        var usuarioEstadoModulo = await _context.UsuarioEstadoModulos
            .AnyAsync(uem => uem.EstadoId == id);
        var usuarioEstadoVideo = await _context.UsuarioEstadoVideos
            .AnyAsync(uev => uev.EstadoId == id);

        if (usuarioEstadoCurso || usuarioEstadoModulo || usuarioEstadoVideo)
            permitirExcluir = false;

        if (!permitirExcluir)
        {
            TempData["Warning"] = $"O estado \"{estado.Nome}\" não pode ser excluído pois já existem registros na(s) tabela(s): \"{(usuarioEstadoCurso ? "UsuarioEstadoCursos" : "")}  {(usuarioEstadoModulo ? "UsuarioEstadoModulos" : "")} {(usuarioEstadoVideo ? "UsuarioEstadoVideos" : "")}\" associados a ele!";
            return RedirectToAction(nameof(Index));
        }

        await _service.Delete(estado.Id);
        TempData["Success"] = $"O estado '{estado.Nome}' foi excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
