using System.Security.Claims;
using FDevs.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AppDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
            ViewBag.User = currentUser;
            return View();
        }
    }
}
