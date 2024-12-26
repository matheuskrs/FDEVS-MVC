using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;
        private readonly IUsuarioService _userService;
        private readonly ICursoService _cursoService;
        private readonly IUsuarioCursoService _usuarioCursoService;
        private readonly ITrilhaService _trilhaService;
        private readonly IEstadoVideoService _estadoVideo;
        private readonly IEstadoCursoService _estadoCurso;
        private readonly IProgressoService _progressoService;
        private readonly IEstadoService _estadoService;
        private readonly IVideoService _videoService;

        public HomeController(
            AppDbContext context, 
            IUsuarioService userService, 
            ICursoService cursoService,
            IUsuarioCursoService usuarioCursoService,
            ITrilhaService trilhaService,
            IEstadoCursoService estadoCurso,
            IEstadoVideoService estadoVideo,
            IProgressoService progressoService,
            IEstadoService estadoService,
            IVideoService videoService
            )
        {
            _context = context;
            _userService = userService;
            _cursoService = cursoService;
            _usuarioCursoService = usuarioCursoService;
            _trilhaService = trilhaService;
            _estadoVideo = estadoVideo;
            _estadoCurso = estadoCurso;
            _progressoService = progressoService;
            _estadoService = estadoService;
            _videoService = videoService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userService.GetUsuarioLogado();
            var currentUserId = currentUser.UsuarioId;
            ViewBag.User = currentUser;
            var progresso = await _progressoService.ObterProgressoAsync(currentUserId);

            HomeVM home = new()
            {
                Cursos = await _usuarioCursoService.GetCursosPorUsuarioAsync(currentUserId),
                Trilhas = await _trilhaService.GetTrilhasPorUsuarioAsync(currentUserId),
                UsuarioEstadoVideos = progresso.UsuarioEstadoVideos,
                UsuarioEstadoCursos = progresso.UsuarioEstadoCursos,
                Estados = await _estadoService.GetEstadosAsync(),
                Videos = await _videoService.GetVideosAsync()
            };
            return View(home);
        }

        public async Task<IActionResult> Details(int id, int? videoId)
        {
            var currentUser = await _userService.GetUsuarioLogado();
            var currentUserId = currentUser.UsuarioId;
            ViewBag.User = currentUser;

            Curso curso = await _cursoService.GetCursoByIdAsync(id);

            List<Modulo> modulos = curso.Modulos.ToList();

            List<Video> videos = await _videoService.GetVideosByCursoIdAsync(id);

            if (!videos.Any()) return RedirectToAction("Index");
            int selectedVideoId = videoId ?? videos.FirstOrDefault().Id;
            var prova = curso.Provas.SingleOrDefault();
            int? questaoAtualId = prova?.Questoes?.FirstOrDefault()?.Id;

            var videoAtual = await _videoService.GetVideoByIdAsync(selectedVideoId);
            var videoAnterior = await _videoService.GetVideoAnteriorAsync(selectedVideoId);
            var proximoVideo = await _videoService.GetProximoVideoAsync(selectedVideoId);
            var usuarioEstadoVideo = await _estadoVideo.GetUsuarioEstadoVideosByIdAsync(currentUserId, videoAtual.Id);
            await _progressoService.UpdateProgressoVideoParaAndamentoAsync(usuarioEstadoVideo);
            List<UsuarioEstadoVideo> usuarioEstadoVideos = await _estadoVideo.GetUsuarioEstadoVideosAsync(); // cria dentro dessa função uma chamada pra atualizar o estado do modulo (e consequentemente o estado do curso).

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
            var video = await _context.Videos
                .Include(v => v.Modulo)
                .SingleOrDefaultAsync(v => v.Id == id);

            var usuario = await _userService.GetUsuarioLogado();
            var usuarioId = usuario.UsuarioId;

            var usuarioEstadoVideo = await _context.UsuarioEstadoVideos
                .SingleOrDefaultAsync(uev => uev.VideoId == id && uev.UsuarioId == usuarioId);

            if(usuarioEstadoVideo.EstadoId == 1) return RedirectToAction("Details", new { id = video.Modulo.CursoId, videoId = video.Id });


            var modulo = video.Modulo;
            var videosDoModulo = await _context.Videos
                .Where(v => v.ModuloId == modulo.Id)
                .Include(v => v.Modulo)
                .ThenInclude(m => m.Curso)
                .ToListAsync();

            var algumVideoEmAndamento = videosDoModulo
                .Any(v => _context.UsuarioEstadoVideos
                        .SingleOrDefault(uev => uev.VideoId == v.Id && uev.UsuarioId == usuarioId).EstadoId == 1
                    );

            var usuarioEstadoModulo = await _context.UsuarioEstadoModulos
                .FirstOrDefaultAsync(uem => uem.ModuloId == modulo.Id && uem.UsuarioId == usuarioId);

            if (usuarioEstadoModulo.EstadoId == 3) // lembra que toda vez que atualiza o video, tem que atualizar o modulo tbm.
            {
                _context.Remove(usuarioEstadoModulo);
                await _context.SaveChangesAsync();

                var novoUsuarioEstadoModulo = new UsuarioEstadoModulo
                {
                    UsuarioId = usuarioId,
                    ModuloId = usuarioEstadoModulo.ModuloId,
                    EstadoId = 1
                };

                _context.Add(novoUsuarioEstadoModulo);
                await _context.SaveChangesAsync();
            }

            var curso = modulo.Curso;
            var modulosDoCurso = await _context.Modulos
                .Where(m => m.CursoId == curso.Id)
                .ToListAsync();

            var algumModuloEmAndamento = modulosDoCurso.Any(m => _context.UsuarioEstadoModulos
                .SingleOrDefault(uem => uem.ModuloId == m.Id && uem.UsuarioId == usuarioId).EstadoId == 1);

            var usuarioEstadoCurso = await _context.UsuarioEstadoCursos
                .FirstOrDefaultAsync(uec => uec.CursoId == curso.Id && uec.UsuarioId == usuarioId);

            if (usuarioEstadoCurso == null)
            {
                usuarioEstadoCurso = new UsuarioEstadoCurso
                {
                    UsuarioId = usuarioId,
                    CursoId = curso.Id,
                    EstadoId = algumModuloEmAndamento ? 1 : 3
                };
                _context.Add(usuarioEstadoCurso);
            }
            else
            {
                if (algumModuloEmAndamento)
                {
                    _context.Remove(usuarioEstadoCurso);
                    await _context.SaveChangesAsync();

                    var novoUsuarioEstadoCurso = new UsuarioEstadoCurso
                    {
                        UsuarioId = usuarioId,
                        CursoId = usuarioEstadoCurso.CursoId,
                        EstadoId = 1
                    };

                    _context.Add(novoUsuarioEstadoCurso);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = video.Modulo.CursoId, videoId = video.Id });
        }

        public async Task<IActionResult> UpdateProgressToComplete(int id)
        {
            var video = await _context.Videos
                .Include(v => v.Modulo)
                .SingleOrDefaultAsync(v => v.Id == id);
            var usuario = await _userService.GetUsuarioLogado();
            var usuarioId = usuario.UsuarioId;
            var usuarioEstadoVideo = await _context.UsuarioEstadoVideos
                .SingleOrDefaultAsync(uev => uev.VideoId == id && uev.UsuarioId == usuarioId);

            if (usuarioEstadoVideo.EstadoId == 2)
            {
                return NoContent();
            }
            _context.Remove(usuarioEstadoVideo);
            await _context.SaveChangesAsync();

            var novoUsuarioEstadoVideo = new UsuarioEstadoVideo
            {
                UsuarioId = usuarioId,
                VideoId = id,
                EstadoId = 2
            };

            _context.Add(novoUsuarioEstadoVideo);
            await _context.SaveChangesAsync();

            var totalVideosModulo = await _context.Videos
                .Where(v => v.ModuloId == video.ModuloId)
                .CountAsync();

            var totalVideosCompletosUsuario = await _context.UsuarioEstadoVideos
                .Where(uev => uev.UsuarioId == usuarioId
                              && uev.EstadoId == 2
                              && _context.Videos.Any(v => v.Id == uev.VideoId && v.ModuloId == video.ModuloId))
                .CountAsync();

            if (totalVideosModulo == totalVideosCompletosUsuario)
            {
                var usuarioEstadoModulo = await _context.UsuarioEstadoModulos
                    .SingleOrDefaultAsync(uem => uem.ModuloId == video.ModuloId && uem.UsuarioId == usuarioId);

                _context.Remove(usuarioEstadoModulo);
                await _context.SaveChangesAsync();

                var novoUsuarioEstadoModulo = new UsuarioEstadoModulo
                {
                    UsuarioId = usuarioId,
                    ModuloId = video.ModuloId,
                    EstadoId = 2
                };

                _context.Add(novoUsuarioEstadoModulo);
                await _context.SaveChangesAsync();

                var cursoId = video.Modulo.CursoId;
                var totalModulosCurso = await _context.Modulos.CountAsync(m => m.CursoId == cursoId);
                var totalModulosCompletosUsuario = await _context.UsuarioEstadoModulos
                    .CountAsync(uem => uem.UsuarioId == usuarioId && uem.EstadoId == 2 && uem.Modulo.CursoId == cursoId);

                if (totalModulosCurso == totalModulosCompletosUsuario)
                {
                    var usuarioEstadoCurso = await _context.UsuarioEstadoCursos
                        .FirstOrDefaultAsync(uec => uec.CursoId == cursoId && uec.UsuarioId == usuarioId);

                    _context.Remove(usuarioEstadoCurso);
                    await _context.SaveChangesAsync();

                    var novoUsuarioEstadoCurso = new UsuarioEstadoCurso
                    {
                        UsuarioId = usuarioId,
                        CursoId = usuarioEstadoCurso.CursoId,
                        EstadoId = 2
                    };

                    _context.Add(novoUsuarioEstadoCurso);
                    await _context.SaveChangesAsync();
                }
            }


            return RedirectToAction("Details", new { id = video.Modulo.CursoId, videoId = video.Id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}