using FDevs.Data;
using FDevs.Models;
using FDevs.Services.ArquivoService;
using FDevs.Services.CursoService;
using FDevs.Services.ExclusaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{



    [Authorize(Roles = "Administrador")]
    public class CursosController : Controller
    {
        private readonly ILogger<CursosController> _logger;
        private readonly AppDbContext _context;
        private readonly ICursoService _service;
        private readonly IArquivoService _arquivoService;
        private readonly IExclusaoService _deleteService;
        private readonly IWebHostEnvironment _host;


        public CursosController(ILogger<CursosController> logger, AppDbContext context, ICursoService service, IArquivoService arquivoService, IExclusaoService deleteService, IWebHostEnvironment host)
        {
            _logger = logger;
            _context = context;
            _service = service;
            _arquivoService = arquivoService;
            _deleteService = deleteService;
            _host = host;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cursos = await _service.GetCursosAsync();
            return View(cursos);
        }

        public async Task<IActionResult> Details(int id)
        {
            ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
            var curso = await _service.GetCursoByIdAsync(id);
            return View(curso);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso, IFormFile Arquivo)
        {
            if (!ModelState.IsValid) return View(curso);

            try
            {
                if (Arquivo != null)
                {
                    string fileName = curso.Id + Path.GetExtension(Arquivo.FileName);
                    string caminho = "img\\Cursos";
                    curso.Foto = await _arquivoService.SalvarArquivoAsync(Arquivo, caminho, fileName);
                }

                await _service.Create(curso);
                TempData["Success"] = $"O curso '{curso.Nome}' foi criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro, o curso não foi criado, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
            if (_context.Cursos == null) return RedirectToAction("Index");
            var curso = await _service.GetCursoByIdAsync(id);
            return View(curso);
        }

        public async Task<IActionResult> EditConfirmed(int id, Curso curso, IFormFile Arquivo)
        {
            var cursoExistente = await _service.GetCursoAsNoTracking(curso.Id);
            ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome", curso.TrilhaId);

            if (id != curso.Id) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View(curso);

            if (Arquivo != null)
            {
                string fileName = curso.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = "img\\Cursos";
                curso.Foto = await _arquivoService.SalvarArquivoAsync(Arquivo, caminho, fileName);
            }
            else
            {
                curso.Foto = cursoExistente.Foto;
            }

            await _service.Update(curso);
            TempData["Success"] = $"O curso '{curso.Nome}' foi alterado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var curso = await _service.GetCursoByIdAsync(id);
            if (curso == null) return RedirectToAction("Index");
            ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
            return View(curso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Curso curso = await _service.GetCursoByIdAsync(id);
            string mensagemErro = _deleteService.PermitirExcluirCurso(curso);

            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction(nameof(Index));
            }

            await _service.Delete(curso.Id);
            TempData["Success"] = $"O curso '{curso.Nome}' foi excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}