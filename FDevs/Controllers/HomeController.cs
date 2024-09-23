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
        Cursos = _context.Cursos
            .Include(c => c.Status)
            .ToList(),
    };
    return View(home);
}

    public IActionResult Details(int id)
    {
        return View(id);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Testar()
    {
        var usuarios = _context.Usuarios.Select(u => new { u.UsuarioId, u.Nome, u.DataNascimento }).ToList();
        return Json(usuarios);
    }
}