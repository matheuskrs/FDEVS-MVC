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

    public IActionResult Details(int id)
    {
        var curso = _context.Cursos
            .Include(c => c.Status)
            .Include(c => c.Trilha)
            .SingleOrDefault(c => c.Id == id);

        var modulos = _context.Modulos
            .Include(m => m.Curso)
            .Include(m => m.Status)
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
            Anterior = _context.Cursos
                .OrderByDescending(j => j.Id)
                .FirstOrDefault(j => j.Id < id),
            Proximo = _context.Cursos
                .OrderBy(p => p.Id)
                .FirstOrDefault(j => j.Id > id),
            Modulos = modulos,
            Videos = videos
        };

        return View(details);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}