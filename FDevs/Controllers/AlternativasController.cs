using FDevs.Data;
using FDevs.Models;
using FDevs.Services.AlternativaService;
using FDevs.Services.ExclusaoService;
using FDevs.Services.QuestaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AlternativasController : Controller
    {
        private readonly IAlternativaService _service;
        private readonly IExclusaoService _deleteService;
        private readonly IQuestaoService _questaoService;

        public AlternativasController(IAlternativaService service, IExclusaoService deleteService, IQuestaoService questaoService)
        {
            _service = service;
            _deleteService = deleteService;
            _questaoService = questaoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["QuestaoId"] = new SelectList(await _questaoService.GetQuestoesAsync(), "Id", "Texto");
            List<Alternativa> alternativas = await _service.GetAlternativasAsync();
            return View(alternativas);
        }

        public async Task<IActionResult> Details(int id)
        {
            Alternativa alternativa = await _service.GetAlternativaByIdAsync(id);
            if (alternativa == null) return RedirectToAction("Index");
            ViewData["QuestaoId"] = new SelectList(await _questaoService.GetQuestoesAsync(), "Id", "Texto");
            return View(alternativa);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["QuestaoId"] = new SelectList(await _questaoService.GetQuestoesAsync(), "Id", "Texto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Alternativa alternativa)
        {
            if (!ModelState.IsValid) return View(alternativa);
            try
            {
                await _service.Create(alternativa);
                TempData["Success"] = $"A alternativa \"{alternativa.Texto}\" foi criada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao criar a alternativa, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Alternativa alternativa = await _service.GetAlternativaByIdAsync(id);
            if (alternativa == null) return RedirectToAction("Index");
            ViewData["QuestaoId"] = new SelectList(await _questaoService.GetQuestoesAsync(), "Id", "Texto");
            return View(alternativa);
        }

        public async Task<IActionResult> EditConfirmed(Alternativa alternativa)
        {
            if (!ModelState.IsValid) return View(alternativa);
            try
            {
                await _service.Update(alternativa);
                TempData["Success"] = $"A alternativa \"{alternativa.Texto}\" foi alterada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao atualizar a alternativa, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["QuestaoId"] = new SelectList(await _questaoService.GetQuestoesAsync(), "Id", "Texto");
            Alternativa alternativa = await _service.GetAlternativaByIdAsync(id);
            if (alternativa == null) return RedirectToAction("Index");
            return View(alternativa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Alternativa alternativa = await _service.GetAlternativaByIdAsync(id);
            if (alternativa == null) return RedirectToAction("Index");

            string mensagemErro = _deleteService.PermitirExcluirAlternativa(alternativa);

            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction("Index");
            }
            try
            {
                await _service.Delete(id);
                TempData["Success"] = $"A alternativa \"{alternativa.Texto}\" foi excluída com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao excluir a alternativa, tente novamente. Detalhes do erro: {ex.Message}";
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