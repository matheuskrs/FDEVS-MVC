using FDevs.Data;
using FDevs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<AdminController> _logger;
    private readonly IUsuarioService _userService;

    public AdminController(AppDbContext context, ILogger<AdminController> logger, IUsuarioService userService)
    {
        _context = context;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userService.GetUsuarioLogado();
        ViewBag.User = currentUser;
        return View();
    }
}

