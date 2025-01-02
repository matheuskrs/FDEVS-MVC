using FDevs.Data;
using FDevs.Models;
using FDevs.Services.EstadoService;
using FDevs.Services.EstadoVideoService;
using FDevs.Services.ModuloService;
using FDevs.Services.UsuarioCursoService;
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
        private readonly IVideoService _service;
        private readonly IModuloService _moduloService;
        private readonly IEstadoService _estadoService;
        private readonly IEstadoVideoService _usuarioEstadoVideo;
        private readonly IUsuarioCursoService _usuarioCursoService;

        public VideosController(
            IVideoService service,
            IModuloService moduloService,
            IEstadoService estadoService,
            IEstadoVideoService usuarioEstadoVideo,
            IUsuarioCursoService usuarioCursoService)
        {
            _service = service;
            _moduloService = moduloService;
            _estadoService = estadoService;
            _usuarioEstadoVideo = usuarioEstadoVideo;
            _usuarioCursoService = usuarioCursoService;
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
            Modulo modulo = await _moduloService.GetModuloByIdAsync(video.ModuloId);

            var usuariosCurso = await _usuarioCursoService.GetCursosPorCursoId(modulo.CursoId);

            try
            {
                await _service.Create(video);
                foreach (var usuarioCurso in usuariosCurso)
                {
                    var usuarioEstadoVideo = new UsuarioEstadoVideo
                    {
                        UsuarioId = usuarioCurso.UsuarioId,
                        VideoId = video.Id,
                        EstadoId = 3
                    };
                    await _usuarioEstadoVideo.CreateUsuarioEstadoVideo(usuarioEstadoVideo);
                }
                TempData["Success"] = $"O vídeo \"{video.Titulo}\" foi criado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"O vídeo não pode ser criado, tente novamente. Detalhes do erro: {ex.Message} ";
                return RedirectToAction("Index");
            }
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

            var usuariosEstadoVideo = await _usuarioEstadoVideo.GetUsuarioEstadoVideosByVideoId(video.Id);

            foreach (var estado in usuariosEstadoVideo)
            {
                await _usuarioEstadoVideo.Delete(estado);
            }
            try
            {
                await _service.Delete(id);
                TempData["Success"] = $"O vídeo \"{video.Titulo}\" foi excluído com sucesso!";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Não foi possível excluir o vídeo, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}