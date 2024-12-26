using FDevs.Services.UsuarioService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IUsuarioService _userService;

        public AdminController(IUsuarioService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userService.GetUsuarioLogado();
            ViewBag.User = currentUser;
            return View();
        }
    }

}