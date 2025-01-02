using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FDevs.Models;
using FDevs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FDevs.Services.UsuarioService;
using FDevs.Services.CursoService;
using FDevs.Services.UsuarioCursoService;
using FDevs.Services.TrilhaService;
using FDevs.Services.EstadoCursoService;
using FDevs.Services.EstadoModuloService;
using FDevs.Services.EstadoVideoService;
using FDevs.Services.ProgressoService;
using FDevs.Services.EstadoService;
using FDevs.Services.VideoService;

namespace FDevs.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUsuarioService _userService;
        private readonly ICursoService _cursoService;
        private readonly IUsuarioCursoService _usuarioCursoService;
        private readonly ITrilhaService _trilhaService;
        private readonly IEstadoVideoService _estadoVideoService;
        private readonly IEstadoCursoService _estadoCursoService;
        private readonly IEstadoModuloService _estadoModuloService;
        private readonly IProgressoService _progressoService;
        private readonly IEstadoService _estadoService;
        private readonly IVideoService _videoService;

        public HomeController(
            IUsuarioService userService,
            ICursoService cursoService,
            IUsuarioCursoService usuarioCursoService,
            ITrilhaService trilhaService,
            IEstadoCursoService estadoCursoService,
            IEstadoVideoService estadoVideoService,
            IEstadoModuloService estadoModuloService,
            IProgressoService progressoService,
            IEstadoService estadoService,
            IVideoService videoService
            )
        {
            _userService = userService;
            _cursoService = cursoService;
            _usuarioCursoService = usuarioCursoService;
            _trilhaService = trilhaService;
            _estadoCursoService = estadoCursoService;
            _estadoVideoService = estadoVideoService;
            _estadoModuloService = estadoModuloService;
            _progressoService = progressoService;
            _estadoService = estadoService;
            _videoService = videoService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userService.GetUsuarioLogado();
            var usuarioId = currentUser.UsuarioId;
            ViewBag.User = currentUser;
            var progresso = await _progressoService.ObterProgressoAsync(usuarioId);
            progresso = await _progressoService.CalcularProgressosEmPorcentagemAsync(progresso, usuarioId);
            var cursos = await _usuarioCursoService.GetCursosPorUsuarioAsync(usuarioId);
            var trilhas = await _trilhaService.GetTrilhasPorUsuarioAsync(usuarioId);
            var estados = await _estadoService.GetEstadosAsync();
            var videos = await _videoService.GetVideosAsync();

            HomeVM home = new()
            {
                Cursos = cursos,
                Trilhas = trilhas,
                Progresso = progresso,
                Estados = estados,
                Videos = videos
            };
            return View(home);
        }

        public async Task<IActionResult> Details(int id, int? videoId)
        {
            var currentUser = await _userService.GetUsuarioLogado();
            var usuarioId = currentUser.UsuarioId;
            ViewBag.User = currentUser;

            Curso curso = await _cursoService.GetCursoByIdAsync(id);

            List<Modulo> modulos = curso.Modulos.ToList();

            List<Video> videos = await _videoService.GetVideosByCursoIdAsync(id);

            if (!videos.Any()) return RedirectToAction("Index");
            int selectedVideoId = videoId ?? videos.FirstOrDefault().Id;
            var prova = curso.Provas.SingleOrDefault();
            int? questaoAtualId = prova?.Questoes?.FirstOrDefault()?.Id;

            var videoAtual = await _videoService.GetVideoByIdAsync(selectedVideoId);
            if (videoAtual.Modulo.CursoId != id) return RedirectToAction("Index");
            var videoAnterior = await _videoService.GetVideoAnteriorAsync(selectedVideoId);
            var proximoVideo = await _videoService.GetProximoVideoAsync(selectedVideoId);
            var usuarioEstadoVideo = await _estadoVideoService.GetUsuarioEstadoVideosByIdAsync(usuarioId, videoAtual.Id);
            await _estadoVideoService.AtualizarEstadoVideoParaAndamentoAsync(usuarioEstadoVideo);
            var usuarioEstadoModulo = await _estadoModuloService.GetUsuarioEstadoModuloByIdAsync(usuarioId, videoAtual.ModuloId);
            await _estadoModuloService.AtualizarEstadoModulo(usuarioEstadoModulo);
            var usuarioEstadoCurso = await _estadoCursoService.GetUsuarioEstadoCursoByIdAsync(usuarioId, id);
            await _estadoCursoService.AtualizarEstadoCurso(usuarioEstadoCurso);


            IEnumerable<UsuarioEstadoVideo> usuarioEstadoVideos = await _estadoVideoService.GetUsuarioEstadoVideosAsync();

            DetailsVM details = new DetailsVM
            {
                CursoAtual = curso,
                VideoAtual = videoAtual,
                VideoAnterior = videoAnterior,
                ProximoVideo = proximoVideo,
                Modulos = modulos,
                Videos = videos,
                SelectedVideoId = selectedVideoId,
                QuestaoId = questaoAtualId,
                UsuarioEstadoVideos = usuarioEstadoVideos,
            };
            return View(details);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgress(int id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);
            return RedirectToAction("Details", new { id = video.Modulo.CursoId, videoId = video.Id });
        }

        public async Task<IActionResult> UpdateProgressToComplete(int id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);
            var usuario = await _userService.GetUsuarioLogado();
            var usuarioId = usuario.UsuarioId;
            var usuarioEstadoVideo = await _estadoVideoService.GetUsuarioEstadoVideosByIdAsync(usuarioId, id);
            var proximoVideo = await _videoService.GetProximoVideoAsync(id);

            if (usuarioEstadoVideo.EstadoId == 2)
            {
                return RedirectToAction("Details", new
                {
                    id = video.Modulo.CursoId,
                    videoId = proximoVideo?.Id ?? video.Id
                });
            }

            await _estadoVideoService.AtualizarEstadoVideoParaConcluidoAsync(usuarioEstadoVideo);
            var totalVideosModulo = await _estadoVideoService.ObterQuantidadeVideos(video);
            var totalVideosCompletosUsuario = await _estadoVideoService.ObterQuantidadeVideosConcluidos(usuarioId, video);

            if (totalVideosModulo != totalVideosCompletosUsuario)
            {
                return RedirectToAction("Details", new
                {
                    id = video.Modulo.CursoId,
                    videoId = proximoVideo?.Id ?? video.Id
                });
            }

            var usuarioEstadoModulo = await _estadoModuloService.GetUsuarioEstadoModuloByIdAsync(usuarioId, video.ModuloId);
            await _estadoModuloService.AtualizarEstadoModuloParaConcluido(usuarioEstadoModulo);

            var totalModulosCurso = await _estadoModuloService.ObterQuantidadeModulos(video.Modulo);
            var totalModulosCompletosUsuario = await _estadoModuloService.ObterQuantidadeModulosConcluidos(usuarioId, video.Modulo);

            if (totalModulosCurso != totalModulosCompletosUsuario)
            {
                return RedirectToAction("Details", new
                {
                    id = video.Modulo.CursoId,
                    videoId = proximoVideo?.Id ?? video.Id
                });
            }

            var cursoId = video.Modulo.CursoId;
            var usuarioEstadoCurso = await _estadoCursoService.GetUsuarioEstadoCursoByIdAsync(usuarioId, cursoId);
            await _estadoCursoService.AtualizarEstadoCursoParaConcluido(usuarioEstadoCurso);

            return RedirectToAction("Details", new
            {
                id = video.Modulo.CursoId,
                videoId = proximoVideo?.Id ?? video.Id
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}