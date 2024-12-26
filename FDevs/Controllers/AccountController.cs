using Microsoft.AspNetCore.Mvc;
using FDevs.ViewModels;
using FDevs.Data;
using FDevs.Services.UsuarioService;

namespace FDevs.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly AppDbContext _context;

        public AccountController(
            ILogger<AccountController> logger,
            IUsuarioService usuarioService,
            AppDbContext context
        )
        {
            //Url.Action
            _logger = logger;
            _usuarioService = usuarioService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            LoginVM login = new()
            {
                UrlRetorno = returnUrl ?? Url.Content("~/")
            };
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                var result = await _usuarioService.LoginUsuario(login);
                if (result.Succeeded)
                    return LocalRedirect(login.UrlRetorno);
                if (result.IsLockedOut)
                    return RedirectToAction("Lockout");
                else
                    ModelState.AddModelError(string.Empty, "Usuário e/ou Senha Inválidos!!!");
            }
            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _usuarioService.LogoffUsuario();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            RegistroVM register = new();
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroVM register)
        {
            register.Enviado = false;
            if (ModelState.IsValid)
            {
                var result = await _usuarioService.RegistrarUsuario(register);
                if (result != null)
                {
                    foreach (var error in result)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                register.Enviado = result == null;
            }
            return View(register);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}