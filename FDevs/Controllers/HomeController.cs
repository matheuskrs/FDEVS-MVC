using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;
using FDevs.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FDevs.Services;

namespace FDevs.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly IUsuarioService _userService;

    public HomeController(ILogger<HomeController> logger, AppDbContext context, IUsuarioService userService)
    {
        _logger = logger;
        _context = context;
        _userService = userService;
    }
    
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userService.GetUsuarioLogado();
        var currentUserId = currentUser.UsuarioId;
        ViewBag.User = currentUser;

        HomeVM home = new()
        {
            Cursos = await _context.UsuarioCursos
                .Include(uc => uc.Curso)
                .ThenInclude(c => c.Estado)
                .Include(uc => uc.Curso.Modulos)
                .ThenInclude(m => m.Videos)
                .Where(uc => uc.UsuarioId == currentUserId)
                .ToListAsync(),

            Usuarios = await _context.Usuarios
                .Include(c => c.Cursos)
                .ThenInclude(uc => uc.Curso)
                .ToListAsync(),
            Trilhas = await _context.Trilhas.ToListAsync(),

        };
        return View(home);
    }

    public async Task<IActionResult> Details(int id, int? videoId)
    {
        var curso = await _context.Cursos
            .Include(c => c.Estado)
            .Include(c => c.Trilha)
            .Include(c => c.Provas)
            .ThenInclude(p => p.Questoes)
            .SingleOrDefaultAsync(c => c.Id == id);

        var modulos = await _context.Modulos
            .Include(m => m.Curso)
            .Include(m => m.Estado)
            .Include(m => m.Videos)
            .Where(m => m.CursoId == id)
            .ToListAsync();

        var videos = await _context.Videos
            .Include(v => v.Modulo)
            .Include(v => v.Estado)
            .Where(v => modulos.Select(m => m.Id).Contains(v.ModuloId))
            .ToListAsync();

        int selectedVideoId = videoId ?? videos.FirstOrDefault().Id;
        var videoAtual = videos.SingleOrDefault(v => v.Id == selectedVideoId);
        var prova = curso.Provas.SingleOrDefault();
        var questaoAtualId = prova.Questoes.FirstOrDefault().Id;

        DetailsVM details = new DetailsVM
        {
            CursoAtual = curso,
            VideoAtual = videoAtual,
            VideoAnterior = await _context.Videos
                .Include(a => a.Modulo)
                .ThenInclude(m => m.Curso)
                .OrderByDescending(v => v.Id)
                .FirstOrDefaultAsync(v => v.Id < selectedVideoId),
            ProximoVideo = await _context.Videos
                .Include(p => p.Modulo)
                .ThenInclude(m => m.Curso)
                .OrderBy(v => v.Id)
                .FirstOrDefaultAsync(v => v.Id > selectedVideoId),
            Modulos = modulos,
            Videos = videos,
            SelectedVideoId = selectedVideoId,
            QuestaoId = questaoAtualId
        };

        return View(details);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProgress(int id)
    {
        var video = await _context.Videos
            .Include(v => v.Modulo)
            .ThenInclude(m => m.Curso)
            .SingleOrDefaultAsync(v => v.Id == id);

        var modulo = video.Modulo;
        var videosDoModulo = await _context.Videos.Where(v => v.ModuloId == modulo.Id).ToListAsync();

        if (videosDoModulo.All(v => v.EstadoId == 1 || v.EstadoId == 2))
        {
            modulo.EstadoId = 1;
            var curso = modulo.Curso;
            var modulosDoCurso = await _context.Modulos.Where(m => m.CursoId == curso.Id).ToListAsync();
            if (modulosDoCurso.All(m => m.EstadoId == 1 || m.EstadoId == 2))
            {
                curso.EstadoId = 1;
            }
        }

        if (video != null)
        {
            if (video.EstadoId == 3)
            {
                video.EstadoId = 1;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new { id = video.Modulo.CursoId, videoId = video.Id });
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<bool> UpdateProgressToCompleted(int id)
    {
        var video = await _context.Videos.Include(v => v.Modulo).FirstOrDefaultAsync(v => v.Id == id);

        if (video != null)
        {
            video.EstadoId = 2;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}