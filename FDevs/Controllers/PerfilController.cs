using FDevs.Data;
using FDevs.Models;
using FDevs.Services.ArquivoService;
using FDevs.Services.UsuarioService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioService _userService;
        private readonly IArquivoService _arquivoService;

        public PerfilController(AppDbContext context, IUsuarioService userService, IArquivoService arquivoService)
        {
            _context = context;
            _userService = userService;
            _arquivoService = arquivoService;
        }

        public async Task<IActionResult> Edit(string id)
        {

            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.SingleOrDefaultAsync(c => c.UsuarioId == id);
            ViewBag.User = usuario;
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditConfirmed(Usuario usuario, IFormFile Arquivo)
        {

            var usuarioExistente = await _context.Usuarios.AsNoTracking().SingleOrDefaultAsync(c => c.UsuarioId == usuario.UsuarioId); ;
            if (ModelState.IsValid)
            {
                if (Arquivo != null)
                {
                    string fileName = usuario.UsuarioId + Path.GetExtension(Arquivo.FileName);
                    string caminho = "img\\Usuarios";
                    usuario.Foto = await _arquivoService.SalvarArquivoAsync(Arquivo, caminho, fileName);
                }
                else
                {
                    usuario.Foto = usuarioExistente.Foto;
                }
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(usuario);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
