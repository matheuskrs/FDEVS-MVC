using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;
[Authorize]
public class PerfilController : Controller
{
    private readonly ILogger<PerfilController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public PerfilController(ILogger<PerfilController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;
    }

    public async Task<IActionResult> Edit(string id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;

        if (_context.Usuarios == null)
        {
            return NotFound();
        }
        var usuario = await _context.Usuarios.SingleOrDefaultAsync(c => c.UsuarioId == id);
        return View(usuario);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditConfirmed(string id, Usuario usuario, IFormFile Arquivo)
    {

        if (ModelState.IsValid)
        {

            if (Arquivo != null)
            {
                string fileName = usuario.UsuarioId + Path.GetExtension(Arquivo.FileName);
                string caminho = Path.Combine(_host.WebRootPath, "img\\Usuarios");
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                string novoArquivo = Path.Combine(caminho, fileName);
                using (var stream = new FileStream(novoArquivo, FileMode.Create))
                {
                    Arquivo.CopyTo(stream);
                }
                usuario.Foto = "\\img\\Usuarios\\" + fileName;
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