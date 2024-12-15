using FDevs.Data;
using FDevs.Models;
using FDevs.Services.CursoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

[Authorize(Roles = "Administrador")]
public class CursosController : Controller
{
    private readonly ILogger<CursosController> _logger;
    private readonly AppDbContext _context;
    private readonly ICursoService _service;
    private readonly IWebHostEnvironment _host;


    public CursosController(ILogger<CursosController> logger, AppDbContext context, ICursoService service, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _service = service;
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
        if (ModelState.IsValid)
        {
            if (Arquivo != null)
            {
                string fileName = curso.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = Path.Combine(_host.WebRootPath, "img\\Cursos");
                string novoArquivo = Path.Combine(caminho, fileName);
                using (var stream = new FileStream(novoArquivo, FileMode.Create))
                {
                    Arquivo.CopyTo(stream);
                }
                curso.Foto = "\\img\\Cursos\\" + fileName;
                await _context.SaveChangesAsync();
            }

            await _service.Create(curso);
            TempData["Success"] = $"O curso {curso.Nome} foi criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
        }
        return View(curso);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
        if (_context.Cursos == null)
        {
            return NotFound();
        }
        var curso = await _service.GetCursoByIdAsync(id);
        return View(curso);
    }

    public async Task<IActionResult> EditConfirmed(int id, Curso curso, IFormFile Arquivo)
    {
        var cursoExistente = await _service.GetCursoAsNoTracking(curso.Id);
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome", curso.TrilhaId);
        if (id != curso.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        { 
            if (Arquivo != null)
            {
                string fileName = curso.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = Path.Combine(_host.WebRootPath, "img\\Cursos");
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                string novoArquivo = Path.Combine(caminho, fileName);
                using (var stream = new FileStream(novoArquivo, FileMode.Create))
                {
                    Arquivo.CopyTo(stream);
                }
                curso.Foto = "\\img\\Cursos\\" + fileName;
            }
            else
            {
                curso.Foto = cursoExistente.Foto;
            }
            await _service.Update(curso);
            TempData["Success"] = $"O curso {curso.Nome} foi alterado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return View(curso);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var curso = await _service.GetCursoByIdAsync(id);
        ViewData["TrilhaId"] = new SelectList(_context.Trilhas, "Id", "Nome");
        if (curso == null)
        {
            return NotFound();
        }
        return View(curso);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var curso = await _service.GetCursoByIdAsync(id);
        if (curso == null) return NotFound();
        var permitirExcluir = true;

        var usuarioCursos = await _context.UsuarioCursos
            .AnyAsync(uc => uc.CursoId == id);

        var modulosCurso = curso.Modulos.Any();

        if(usuarioCursos || modulosCurso)
            permitirExcluir = false;
        

        if (!permitirExcluir)
        {
            TempData["Warning"] = $"O curso \"{curso.Nome}\" não pode ser excluído pois já existem registros na(s) tabela(s): \"{(usuarioCursos ? "USUÁRIOS" : "" )}  {(modulosCurso ? "MÓDULOS" : "")}\" associados a ele!";
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