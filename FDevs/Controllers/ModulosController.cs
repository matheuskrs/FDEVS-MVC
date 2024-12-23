using FDevs.Data;
using FDevs.Models;
using FDevs.Services.ExclusaoService;
using FDevs.Services.ModuloService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{



    [Authorize(Roles = "Administrador")]
    public class ModulosController : Controller
    {
        private readonly ILogger<ModulosController> _logger;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IModuloService _service;
        private readonly IExclusaoService _deleteService;

        public ModulosController(ILogger<ModulosController> logger, AppDbContext context, IWebHostEnvironment host, IModuloService service, IExclusaoService deleteService)
        {
            _logger = logger;
            _context = context;
            _host = host;
            _service = service;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Modulo> modulos = await _service.GetModulosAsync();
            return View(modulos);
        }

        public async Task<IActionResult> Details(int id)
        {
            Modulo modulo = await _service.GetModuloByIdAsync(id);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
            return View(modulo);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Modulo modulo)
        {
            if (!ModelState.IsValid) return View(modulo);

            var usuariosCurso = await _context.UsuarioCursos
                .Where(uc => uc.CursoId == modulo.CursoId)
                .ToListAsync();

            await _service.Create(modulo); // Tem que criar o módulo antes de passar o Id no foreach abaixo, porque antes disso ele não existe no contexto.

            foreach (var usuarioCurso in usuariosCurso)
            {
                var usuarioEstadoModulo = new UsuarioEstadoModulo
                {
                    UsuarioId = usuarioCurso.UsuarioId,
                    ModuloId = modulo.Id,
                    EstadoId = 3
                };
                _context.Add(usuarioEstadoModulo);
            }
            TempData["Success"] = $"O módulo {modulo.Nome} foi criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Modulos == null) return NoContent();
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");

            Modulo modulo = await _service.GetModuloByIdAsync(id);
            return View(modulo);
        }

        public async Task<IActionResult> EditConfirmed(int id, Modulo modulo)
        {
            if (id != modulo.Id) return NotFound();

            if (!ModelState.IsValid) return View(modulo);
            await _service.Update(modulo);
            TempData["Success"] = $"O módulo {modulo.Nome} foi alterado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nome");
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
            Modulo modulo = await _service.GetModuloByIdAsync(id);
            if (modulo == null) return RedirectToAction("Index");
            return View(modulo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Modulo modulo = await _service.GetModuloByIdAsync(id);

            string mensagemErro = _deleteService.PermitirExcluirModulo(modulo);
            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction(nameof(Index));
            }

            foreach (var video in modulo.Videos)
            {
                var usuariosEstadoVideo = await _context.UsuarioEstadoVideos
                    .Where(uev => uev.VideoId == video.Id)
                    .ToListAsync();

                foreach (var estado in usuariosEstadoVideo)
                {
                    _context.Remove(estado);
                }
            }

            var usuariosEstadoModulo = await _context.UsuarioEstadoModulos
                .Where(uem => uem.ModuloId == modulo.Id)
                .ToListAsync();

            foreach (var estado in usuariosEstadoModulo)
            {
                _context.Remove(estado);
            }

            await _service.Delete(id);
            TempData["Success"] = $"O módulo '{modulo.Nome}' foi excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

}