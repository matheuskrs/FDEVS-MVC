using FDevs.Data;
using FDevs.Models;
using FDevs.Services.UsuarioCursoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{



    [Authorize(Roles = "Administrador")]
    public class UsuarioCursosController : Controller
    {
        private readonly ILogger<UsuarioCursosController> _logger;
        private readonly AppDbContext _context;
        private readonly IUsuarioCursoService _usuarioCursoService;

        public UsuarioCursosController(
            ILogger<UsuarioCursosController> logger,
            AppDbContext context,
            IUsuarioCursoService usuarioCursoService)
        {
            _logger = logger;
            _context = context;
            _usuarioCursoService = usuarioCursoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioCursos = await _usuarioCursoService.GetUsuarioCursosAsync();
            return View(usuarioCursos);
        }

        public async Task<IActionResult> Details(string usuarioId, int cursoId)
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos.ToList(), "Id", "Nome");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.ToList(), "UsuarioId", "Nome");
            var usuarioCurso = await _usuarioCursoService.GetCursoPorUsuarioCursoAsync(usuarioId, cursoId);
            if (usuarioCurso == null)
                return NotFound();

            return View(usuarioCurso);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos.ToList(), "Id", "Nome");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.ToList(), "UsuarioId", "Nome");
            ViewBag.Usuarios = await _context.Usuarios.ToListAsync();
            ViewBag.Cursos = await _context.Cursos.ToListAsync();
            return View();
        }

        // Cria um novo relacionamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCurso usuarioCurso)
        {
            if (ModelState.IsValid)
            {
                var videos = await _context.Videos
                    .Include(v => v.Modulo)
                    .ThenInclude(v => v.Curso)
                    .Where(v => v.Modulo.CursoId == usuarioCurso.CursoId)
                    .ToListAsync();

                var modulos = await _context.Modulos
                    .Include(m => m.Curso)
                    .Include(m => m.UsuarioEstadoModulos)
                    .Where(m => m.CursoId == usuarioCurso.CursoId)
                    .ToListAsync();

                var curso = await _context.Cursos
                    .Include(c => c.UsuarioEstadoCursos)
                    .FirstOrDefaultAsync(c => c.Id == usuarioCurso.CursoId);

                var cursoExistente = await _context.UsuarioCursos.SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioCurso.UsuarioId && uc.CursoId == usuarioCurso.CursoId);

                if (cursoExistente != null)
                {
                    TempData["Warning"] = $"Esse usuário não pode ser relacionado a este curso, pois já existem registros na tabela: \"UsuarioCursos\" iguais associados a ele!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var video in videos)
                {
                    var usuarioEstadoVideo = new UsuarioEstadoVideo
                    {
                        UsuarioId = usuarioCurso.UsuarioId,
                        VideoId = video.Id,
                        EstadoId = 3
                    };
                    _context.Add(usuarioEstadoVideo);
                }

                foreach (var modulo in modulos)
                {
                    var usuarioEstadoModulo = new UsuarioEstadoModulo
                    {
                        UsuarioId = usuarioCurso.UsuarioId,
                        ModuloId = modulo.Id,
                        EstadoId = 3
                    };
                    _context.Add(usuarioEstadoModulo);
                }

                var usuarioEstadoCurso = new UsuarioEstadoCurso
                {
                    UsuarioId = usuarioCurso.UsuarioId,
                    CursoId = curso.Id,
                    EstadoId = 3
                };
                _context.Add(usuarioEstadoCurso);
                _context.Add(usuarioCurso);
                await _context.SaveChangesAsync();
                usuarioCurso = await _context.UsuarioCursos
                    .Include(uc => uc.Usuario)
                    .Include(uc => uc.Curso)
                    .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioCurso.UsuarioId && uc.CursoId == usuarioCurso.CursoId);
                TempData["Success"] = $"A relação entre \"{usuarioCurso.Usuario.Nome}\" e \"{usuarioCurso.Curso.Nome}\" foi criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioCurso);
        }

        public async Task<IActionResult> Delete(string usuarioId, int cursoId)
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos.ToList(), "Id", "Nome");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios.ToList(), "UsuarioId", "Nome");
            var usuarioCurso = await _context.UsuarioCursos
                .Include(uc => uc.Usuario)
                .Include(uc => uc.Curso)
                .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CursoId == cursoId);
            if (usuarioCurso == null)
                return NotFound();

            return View(usuarioCurso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string usuarioId, int cursoId)
        {
            var usuarioCurso = await _context.UsuarioCursos
                .Include(uc => uc.Usuario)
                .Include(uc => uc.Curso)
                .SingleOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CursoId == cursoId);
            if (usuarioCurso == null)
                return NotFound();

            var videos = await _context.Videos
                .Include(v => v.Modulo)
                .ThenInclude(v => v.Curso)
                .Where(v => v.Modulo.CursoId == usuarioCurso.CursoId)
                .ToListAsync();

            var modulos = await _context.Modulos
                .Include(m => m.Curso)
                .Include(m => m.UsuarioEstadoModulos)
                .Where(m => m.CursoId == usuarioCurso.CursoId)
                .ToListAsync();

            var curso = await _context.Cursos
                .Include(c => c.UsuarioEstadoCursos)
                .FirstOrDefaultAsync(c => c.Id == usuarioCurso.CursoId);

            foreach (var video in videos)
            {
                var usuarioEstadoVideo = await _context.UsuarioEstadoVideos
                    .FirstOrDefaultAsync(uem => uem.VideoId == video.Id && uem.UsuarioId == usuarioId);
                _context.Remove(usuarioEstadoVideo);
            }

            foreach (var modulo in modulos)
            {
                var usuarioEstadoModulo = await _context.UsuarioEstadoModulos
                    .FirstOrDefaultAsync(uem => uem.ModuloId == modulo.Id && uem.UsuarioId == usuarioId);
                _context.Remove(usuarioEstadoModulo);
            }

            var usuarioEstadoCurso = await _context.UsuarioEstadoCursos
                .FirstOrDefaultAsync(uec => uec.CursoId == curso.Id && uec.UsuarioId == usuarioId);

            _context.Remove(usuarioEstadoCurso);

            _context.Remove(usuarioCurso);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"O relacionamento entre \"{usuarioCurso.Usuario.Nome}\" e o curso \"{usuarioCurso.Curso.Nome}\" foi excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}