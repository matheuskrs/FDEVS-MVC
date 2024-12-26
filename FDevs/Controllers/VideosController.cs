using FDevs.Data;
using FDevs.Models;
using FDevs.Services.EstadoService;
using FDevs.Services.ModuloService;
using FDevs.Services.VideoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class VideosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IVideoService _service;
        private readonly IModuloService _moduloService;
        private readonly IEstadoService _estadoService;

        public VideosController(AppDbContext context, IVideoService service, IModuloService moduloService, IEstadoService estadoService)
        {
            _context = context;
            _service = service;
            _moduloService = moduloService;
            _estadoService = estadoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Video> videos = await _service.GetVideosAsync();
            return View(videos);
        }

        public async Task<IActionResult> Details(int id)
        {
            Video video = await _service.GetVideoByIdAsync(id);
            if (video == null) return RedirectToAction("Index");
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["ModuloId"] = new SelectList(await _moduloService.GetModulosAsync(), "Id", "Nome");
            return View(video);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["ModuloId"] = new SelectList(await _moduloService.GetModulosAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Video video)
        {
            if (!ModelState.IsValid) return View(video);
            Modulo modulo = await _context.Modulos.FirstOrDefaultAsync(m => m.Id == video.ModuloId);

            await _service.Create(video);

            var usuariosCurso = await _context.UsuarioCursos
                .Include(uc => uc.Curso)
                .ThenInclude(c => c.Modulos)
                .Where(uc => uc.CursoId == modulo.CursoId)
                .ToListAsync();
            foreach (var usuarioCurso in usuariosCurso)
            {
                var usuarioEstadoVideo = new UsuarioEstadoVideo
                {
                    UsuarioId = usuarioCurso.UsuarioId,
                    VideoId = video.Id,
                    EstadoId = 3
                };
                _context.Add(usuarioEstadoVideo);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"O vídeo \"{video.Titulo}\" foi criado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Video video = await _service.GetVideoByIdAsync(id);
            if (video == null) return RedirectToAction("Index");
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["ModuloId"] = new SelectList(await _moduloService.GetModulosAsync(), "Id", "Nome");


            return View(video);
        }

        public async Task<IActionResult> EditConfirmed(Video video)
        {
            if (!ModelState.IsValid) return View(video);
            await _service.Update(video);
            TempData["Success"] = $"O vídeo \"{video.Titulo}\" foi alterado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var video = await _service.GetVideoByIdAsync(id);
            if (video == null) return RedirectToAction("Index");
            ViewData["EstadoId"] = new SelectList(await _estadoService.GetEstadosAsync(), "Id", "Nome");
            ViewData["ModuloId"] = new SelectList(await _moduloService.GetModulosAsync(), "Id", "Nome");
            
            return View(video);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Video video = await _service.GetVideoByIdAsync(id);
            if (video == null) return RedirectToAction("Index");

            var usuariosEstadoVideo = await _context.UsuarioEstadoVideos
                .Where(uev => uev.VideoId == video.Id)
                .ToListAsync();

            foreach (var estado in usuariosEstadoVideo)
            {
                _context.Remove(estado);
            }

            await _service.Delete(id);
            TempData["Success"] = $"O vídeo \"{video.Titulo}\" foi excluído com sucesso!";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}