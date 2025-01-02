using FDevs.Data;
using FDevs.Models;
using FDevs.Services.CursoService;
using FDevs.Services.EstadoService;
using FDevs.Services.ExclusaoService;
using FDevs.Services.ModuloService;
using FDevs.Services.UsuarioCursoService;
using FDevs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ModulosController : Controller
    {
        private readonly IModuloService _service;
        private readonly IExclusaoService _deleteService;
        private readonly ICursoService _cursoService;
        private readonly IEstadoService _estadoService;
        private readonly IUsuarioCursoService _usuarioCursoService;
        private readonly AppDbContext _context;

        public ModulosController(IModuloService service, IExclusaoService deleteService, ICursoService cursoService, IEstadoService estadoService, IUsuarioCursoService usuarioCursoService, AppDbContext context)
        {
            _service = service;
            _deleteService = deleteService;
            _cursoService = cursoService;
            _estadoService = estadoService;
            _usuarioCursoService = usuarioCursoService;
            _context = context;
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
            if (modulo == null) return RedirectToAction("Index");
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");
            return View(modulo);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Modulo modulo)
        {
            if (!ModelState.IsValid) return View(modulo);

            var usuariosCurso = await _usuarioCursoService.GetCursosPorCursoId(modulo.CursoId);

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
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Modulo modulo = await _service.GetModuloByIdAsync(id);
            if (modulo == null) return RedirectToAction("Index");
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");

            return View(modulo);
        }

        public async Task<IActionResult> EditConfirmed(Modulo modulo)
        {
            if (modulo == null) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View(modulo);
            await _service.Update(modulo);
            TempData["Success"] = $"O módulo {modulo.Nome} foi alterado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Modulo modulo = await _service.GetModuloByIdAsync(id);
            if (modulo == null) return RedirectToAction("Index");
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");
            return View(modulo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Modulo modulo = await _service.GetModuloByIdAsync(id);
            if (modulo == null) return RedirectToAction("Index");

            string mensagemErro = _deleteService.PermitirExcluirModulo(modulo);
            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

}