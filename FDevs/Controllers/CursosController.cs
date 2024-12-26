using FDevs.Models;
using FDevs.Services.ArquivoService;
using FDevs.Services.CursoService;
using FDevs.Services.ExclusaoService;
using FDevs.Services.TrilhaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CursosController : Controller
    {
        private readonly ICursoService _service;
        private readonly IArquivoService _arquivoService;
        private readonly IExclusaoService _deleteService;
        private readonly ITrilhaService _trilhaService;

        public CursosController(ICursoService service, IArquivoService arquivoService, IExclusaoService deleteService, ITrilhaService trilhaService)
        {
            _service = service;
            _arquivoService = arquivoService;
            _deleteService = deleteService;
            _trilhaService = trilhaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Curso> cursos = await _service.GetCursosAsync();
            return View(cursos);
        }

        public async Task<IActionResult> Details(int id)
        {
            ViewData["TrilhaId"] = new SelectList(await _trilhaService.GetTrilhasAsync(), "Id", "Nome");
            Curso curso = await _service.GetCursoByIdAsync(id);
            return View(curso);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["TrilhaId"] = new SelectList(await _trilhaService.GetTrilhasAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso, IFormFile Arquivo)
        {
            if (!ModelState.IsValid) return View(curso);

            if (Arquivo != null)
            {
                string fileName = curso.Id + Path.GetExtension(Arquivo.FileName);
                string caminho = "img\\Cursos";
                curso.Foto = await _arquivoService.SalvarArquivoAsync(Arquivo, caminho, fileName);
            }
            else
            {
                curso.Foto = "\\img\\Cursos\\sem-foto.png";

            }

            try
            {
                await _service.Create(curso);
                TempData["Success"] = $"O curso \"{curso.Nome}\" foi criado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro, o curso não foi criado, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["TrilhaId"] = new SelectList(await _trilhaService.GetTrilhasAsync(), "Id", "Nome");
            Curso curso = await _service.GetCursoByIdAsync(id);
            if (curso == null) return RedirectToAction("Index");

            return View(curso);
        }

        public async Task<IActionResult> EditConfirmed(int id, Curso curso, IFormFile Arquivo)
        {
            Curso cursoExistente = await _service.GetCursoAsNoTracking(curso.Id);
            ViewData["TrilhaId"] = new SelectList(await _trilhaService.GetTrilhasAsync(), "Id", "Nome", curso.TrilhaId);

            if (curso == null) return RedirectToAction("Index");

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
            try
            {
                await _service.Update(curso);
                TempData["Success"] = $"O curso \"{curso.Nome}\" foi alterado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao tentar alterar o curso, tente novamente. Detalhes do erro: {ex.Message}";
                throw;
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Curso curso = await _service.GetCursoByIdAsync(id);
            if (curso == null) return RedirectToAction("Index");
            ViewData["TrilhaId"] = new SelectList(await _trilhaService.GetTrilhasAsync(), "Id", "Nome");
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
            try
            {
                await _service.Delete(curso.Id);
                TempData["Success"] = $"O curso \"{curso.Nome}\" foi excluído com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao excluir o curso, tente novamente. Detalhes do erro: {ex.Message}";
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