using FDevs.Data;
using FDevs.Models;
using FDevs.Services.CursoService;
using FDevs.Services.ExclusaoService;
using FDevs.Services.ProvaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProvasController : Controller
    {
        private readonly IProvaService _service;
        private readonly ICursoService _cursoService;
        private readonly IExclusaoService _deleteService;

        public ProvasController(IProvaService service, ICursoService cursoService, IExclusaoService deleteService)
        {
            _service = service;
            _cursoService = cursoService;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Prova> provas = await _service.GetProvasAsync();
            return View(provas);
        }

        public async Task<IActionResult> Details(int id)
        {
            Prova prova = await _service.GetProvaByIdAsync(id);
            if (prova == null) return RedirectToAction("Index");
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");
            return View(prova);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prova prova)
        {
            if (!ModelState.IsValid) return View(prova);
            try
            {
                await _service.Create(prova);
                TempData["Success"] = $"A prova \"{prova.Nome}\" foi criada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao tentar criar a prova, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Prova prova = await _service.GetProvaByIdAsync(id);
            if (prova == null) return RedirectToAction("Index");
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");
            return View(prova);
        }

        public async Task<IActionResult> EditConfirmed(int id, Prova prova)
        {
            if (!ModelState.IsValid) return View(prova);
            try
            {
                await _service.Update(prova);
                TempData["Success"] = $"A prova \"{prova.Nome}\" foi alterada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao tentar atualizar a prova, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["CursoId"] = new SelectList(await _cursoService.GetCursosAsync(), "Id", "Nome");
            Prova prova = await _service.GetProvaByIdAsync(id);
            if (prova == null) return RedirectToAction("Index");

            return View(prova);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var prova = await _service.GetProvaByIdAsync(id);
            if (prova == null) return RedirectToAction("Index");

            var mensagemErro = _deleteService.PermitirExcluirProva(prova);
            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction("Index");
            }
            try
            {
                await _service.Delete(id);
                TempData["Success"] = $"A Prova \"{prova.Nome}\" foi excluída com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"A Prova não pode ser excluída, tente novamente. Detalhes do erro: {ex.Message}";
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