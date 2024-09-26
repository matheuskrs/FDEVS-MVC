using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;
using FDevs.ViewModels;

namespace FDevs.Controllers;

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
        HomeVM home = new()
        {
            Usuarios = _context.Usuarios
                .Include(c => c.Cursos)
                .ThenInclude(uc => uc.Curso)
                .ToList(),
            Trilhas = _context.Trilhas.ToList(),
            Cursos = _context.Cursos
                .Include(c => c.Status)
                .ToList(),
        };
        return View(home);
    }

    public IActionResult Details(int id, int? videoId = null)
    {
        var curso = _context.Cursos
            .Include(c => c.Status)
            .Include(c => c.Trilha)
            .SingleOrDefault(c => c.Id == id);

        var modulos = _context.Modulos
            .Include(m => m.Curso)
            .Include(m => m.Status)
            .Include(m => m.Videos)
            .Where(m => m.CursoId == id)
            .ToList();

        var videos = _context.Videos
            .Include(v => v.Modulo)
            .Include(v => v.Status)
            .Where(v => modulos.Select(m => m.Id).Contains(v.ModuloId))
            .ToList();

        DetailsVM details = new DetailsVM
        {
            Atual = curso,
            Modulos = modulos,
            Videos = videos,
            SelectedVideoId = videoId
        };

        return View(details);
    }

    [HttpPost]
    public IActionResult UpdateProgress(int id)
    {
        var video = _context.Videos
            .Include(v => v.Modulo)
            .SingleOrDefault(v => v.Id == id);

        if (video != null)
        {
            if (video.StatusId == 3)
            {
                video.StatusId = 1;
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new { id = video.Modulo.CursoId, videoId = video.Id });
        }
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }



}