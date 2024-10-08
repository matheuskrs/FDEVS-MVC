using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;
using FDevs.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FDevs.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult Index()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;

        HomeVM home = new()
        {
            Cursos = _context.UsuarioCursos
                .Include(uc => uc.Curso)
                .ThenInclude(c => c.Estado)
                .Include(uc => uc.Curso.Modulos)
                .ThenInclude(m => m.Videos)
                .Where(uc => uc.UsuarioId == currentUserId)
                .ToList(),

            Usuarios = _context.Usuarios
                .Include(c => c.Cursos)
                .ThenInclude(uc => uc.Curso)
                .ToList(),
            Trilhas = _context.Trilhas.ToList(),
        };
        return View(home);
    }

    public IActionResult Details(int id, int? videoId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        var curso = _context.Cursos
            .Include(c => c.Estado)
            .Include(c => c.Trilha)
            .Include(c => c.Provas)
            .ThenInclude(p => p.Questoes)
            .SingleOrDefault(c => c.Id == id);

        var modulos = _context.Modulos
            .Include(m => m.Curso)
            .Include(m => m.Estado)
            .Include(m => m.Videos)
            .Where(m => m.CursoId == id)
            .ToList();

        var videos = _context.Videos
            .Include(v => v.Modulo)
            .Include(v => v.Estado)
            .Where(v => modulos.Select(m => m.Id).Contains(v.ModuloId))
            .ToList();
        int selectedVideoId = videoId ?? videos.FirstOrDefault().Id;
        var videoAtual = videos.SingleOrDefault(v => v.Id == selectedVideoId);

        var prova = curso.Provas.SingleOrDefault();
        var questaoAtualId = prova.Questoes.FirstOrDefault().Id;

        DetailsVM details = new DetailsVM
        {
            CursoAtual = curso,
            VideoAtual = videoAtual,
            VideoAnterior = _context.Videos
                .Include(a => a.Modulo)
                .ThenInclude(m => m.Curso)
                .OrderByDescending(v => v.Id)
                .FirstOrDefault(v => v.Id < selectedVideoId),
            ProximoVideo = _context.Videos
                .Include(p => p.Modulo)
                .ThenInclude(m => m.Curso)
                .OrderBy(v => v.Id)
                .FirstOrDefault(v => v.Id > selectedVideoId),
            Modulos = modulos,
            Videos = videos,
            SelectedVideoId = selectedVideoId,
            QuestaoId = questaoAtualId
        };

        return View(details);
    }

    [HttpPost]
    public IActionResult UpdateProgress(int id)
    {
        var video = _context.Videos
            .Include(v => v.Modulo)
            .ThenInclude(m => m.Curso)
            .SingleOrDefault(v => v.Id == id);

        var modulo = video.Modulo;
        var videosDoModulo = _context.Videos.Where(v => v.ModuloId == modulo.Id).ToList();

        if (videosDoModulo.All(v => v.EstadoId == 1 || v.EstadoId == 2))
        {
            modulo.EstadoId = 1;
            var curso = modulo.Curso;
            var modulosDoCurso = _context.Modulos.Where(m => m.CursoId == curso.Id).ToList();
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
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new { id = video.Modulo.CursoId, videoId = video.Id });
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public bool UpdateProgressToCompleted(int id)
    {
        var video = _context.Videos.Include(v => v.Modulo).FirstOrDefault(v => v.Id == id);

        if (video != null)
        {
            video.EstadoId = 2;
            _context.SaveChanges();
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