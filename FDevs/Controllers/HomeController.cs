using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;
using FDevs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FDevs.Services.UsuarioService;

namespace FDevs.Controllers
{



    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IUsuarioService _userService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IUsuarioService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userService.GetUsuarioLogado();
            var currentUserId = currentUser.UsuarioId;
            ViewBag.User = currentUser;

            HomeVM home = new()
            {
                Cursos = await _context.UsuarioCursos
                    .Include(uc => uc.Curso)
                    .ThenInclude(c => c.Modulos)
                    .ThenInclude(m => m.Videos)
                    .Where(uc => uc.UsuarioId == currentUserId)
                    .ToListAsync(),

                Trilhas = await _context.Trilhas
                .Where(t => _context.Cursos
                    .Any(c => c.TrilhaId == t.Id &&
                              _context.UsuarioCursos.Any(uc => uc.CursoId == c.Id && uc.UsuarioId == currentUserId)))
                .ToListAsync(),
                UsuarioEstadoVideos = await _context.UsuarioEstadoVideos
                    .Where(uec => uec.UsuarioId == currentUserId)
                    .ToListAsync(),
                UsuarioEstadoCursos = await _context.UsuarioEstadoCursos
                    .Where(uec => uec.UsuarioId == currentUserId)
                    .ToListAsync(),
                Estados = await _context.Estados.ToListAsync(),
                Videos = await _context.Videos.ToListAsync()
            };
            return View(home);
        }

        public async Task<IActionResult> Details(int id, int? videoId)
        {
            var currentUser = await _userService.GetUsuarioLogado();
            var currentUserId = currentUser.UsuarioId;
            ViewBag.User = currentUser;

            var curso = await _context.Cursos
                .Include(c => c.Provas)
                .SingleOrDefaultAsync(c => c.Id == id);

            var modulos = await _context.Modulos
                .Include(m => m.Videos)
                .Where(m => m.CursoId == id)
                .ToListAsync();

            var videos = await _context.Videos
                .Where(v => modulos.Select(m => m.Id).Contains(v.ModuloId))
                .ToListAsync();

            if (!videos.Any()) return RedirectToAction("Index");
            int selectedVideoId = videoId ?? videos.FirstOrDefault().Id;
            var videoAtual = videos.SingleOrDefault(v => v.Id == selectedVideoId);
            var prova = curso.Provas.SingleOrDefault();
            int? questaoAtualId = prova?.Questoes?.FirstOrDefault()?.Id;


            DetailsVM details = new DetailsVM
            {
                CursoAtual = curso,
                VideoAtual = videoAtual,
                VideoAnterior = await _context.Videos
                    .Include(a => a.Modulo)
                    .ThenInclude(m => m.Curso)
                    .OrderByDescending(a => a.Id)
                    .FirstOrDefaultAsync(a => a.Id < selectedVideoId),
                ProximoVideo = await _context.Videos
                    .Include(p => p.Modulo)
                    .ThenInclude(m => m.Curso)
                    .OrderBy(v => v.Id)
                    .FirstOrDefaultAsync(v => v.Id > selectedVideoId),
                Modulos = modulos,
                Videos = videos,
                SelectedVideoId = selectedVideoId,
                QuestaoId = questaoAtualId,
                UsuarioEstadoVideos = await _context.UsuarioEstadoVideos
                    .Include(uec => uec.Estado)
                    .Include(uec => uec.Video)
                    .ToListAsync(),
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

            if (usuarioEstadoVideo.EstadoId == 3)
            {
                _context.Remove(usuarioEstadoVideo);
                await _context.SaveChangesAsync();

                var novoUsuarioEstadoVideo = new UsuarioEstadoVideo
                {
                    UsuarioId = usuarioId,
                    VideoId = id,
                    EstadoId = 1
                };

                _context.Add(novoUsuarioEstadoVideo);
                await _context.SaveChangesAsync();
            }

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

            if (usuarioEstadoModulo.EstadoId == 3)
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