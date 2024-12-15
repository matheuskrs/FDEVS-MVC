using FDevs.Data;
using FDevs.Models;
using FDevs.Services.VideoService;
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
    private readonly IVideoService _service;

    public VideosController(ILogger<VideosController> logger, AppDbContext context, IVideoService service)
    {
        _logger = logger;
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var videos = await _service.GetVideosAsync();
        return View(videos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var video = await _service.GetVideoByIdAsync(id);
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["ModuloId"] = new SelectList(_context.Modulos, "Id", "Nome");
        return View(video);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var modulos = await _context.Modulos
            .Include(m => m.Curso)
            .OrderBy(m => m.Curso.Nome)
            .ToListAsync();
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
            var modulo = await _context.Modulos.FindAsync(video.ModuloId);

            await _service.Create(video);

            var usuariosCurso = await _context.UsuarioCursos
                .Include(uc => uc.Curso)
                .ThenInclude(c => c.Modulos)
                .Where(uc => uc.CursoId == video.Modulo.CursoId)
                .ToListAsync();
            foreach (var usuarioCurso in usuariosCurso)
            {
                var usuarioEstadoVideo = new UsuarioEstadoVideo
                {
                    UsuarioId = usuarioCurso.UsuarioId,
                    VideoId = video.Id,
                    EstadoId = 3
                };
                _context.Add(usuarioEstadoVideo);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"O vídeo {video.Titulo} foi criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(video);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["ModuloId"] = new SelectList(_context.Modulos, "Id", "Nome");

        var video = await _service.GetVideoByIdAsync(id);
        return View(video);
    }

    public async Task<IActionResult> EditConfirmed(Video video)
    {
        if (ModelState.IsValid)
        {
            await _service.Update(video);
            TempData["Success"] = $"O vídeo {video.Titulo} foi alterado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return View(video);
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
        ViewData["ModuloId"] = new SelectList(_context.Modulos, "Id", "Nome");
        var video = await _service.GetVideoByIdAsync(id);
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
        var video = await _service.GetVideoByIdAsync(id);
        if (video == null) return NotFound();

        var usuariosEstadoVideo = await _context.UsuarioEstadoVideos
            .Where(uev => uev.VideoId == video.Id)
            .ToListAsync();

        foreach (var estado in usuariosEstadoVideo)
        {
            _context.Remove(estado);
        }

        await _service.Delete(id);
        TempData["Success"] = $"O vídeo '{video.Titulo}' foi excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
