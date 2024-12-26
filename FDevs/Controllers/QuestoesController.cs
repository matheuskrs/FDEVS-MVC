using FDevs.Models;
using FDevs.Services.ExclusaoService;
using FDevs.Services.ProvaService;
using FDevs.Services.QuestaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class QuestoesController : Controller
    {
        private readonly IQuestaoService _service;
        private readonly IProvaService _provaService;
        private readonly IExclusaoService _deleteService;

        public QuestoesController(IQuestaoService service, IProvaService provaService, IExclusaoService deleteService)
        {
            _service = service;
            _provaService = provaService;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ProvaId"] = new SelectList(await _provaService.GetProvasAsync(), "Id", "Nome");
            List<Questao> questoes = await _service.GetQuestoesAsync();
            return View(questoes);
        }

        public async Task<IActionResult> Details(int id)
        {
            Questao questao = await _service.GetQuestaoByIdAsync(id);
            if (questao == null) return RedirectToAction("Index");
            ViewData["ProvaId"] = new SelectList(await _provaService.GetProvasAsync(), "Id", "Nome");


            return View(questao);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ProvaId"] = new SelectList(await _provaService.GetProvasAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Questao questao)
        {
            if (!ModelState.IsValid) return View(questao);
            try
            {
                await _service.Create(questao);
                TempData["Success"] = $"A questão {questao.Texto} foi criada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao criar a sua questão, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Questao questao = await _service.GetQuestaoByIdAsync(id);
            if (questao == null) return RedirectToAction("Index");
            ViewData["ProvaId"] = new SelectList(await _provaService.GetProvasAsync(), "Id", "Nome");
            return View(questao);
        }

        public async Task<IActionResult> EditConfirmed(Questao questao)
        {
            if (!ModelState.IsValid) return View(questao);
            try
            {
                await _service.Update(questao);
                TempData["Success"] = $"A questão foi alterada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao alterar a questão, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Questao questao = await _service.GetQuestaoByIdAsync(id);
            if (questao == null) return RedirectToAction("Index");
            ViewData["ProvaId"] = new SelectList(await _provaService.GetProvasAsync(), "Id", "Nome");
            return View(questao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Questao questao = await _service.GetQuestaoByIdAsync(id);
            if (questao == null) return RedirectToAction("Index");

            string mensagemErro = _deleteService.PermitirExcluirQuestao(questao);

            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction("Index");
            }
            try
            {
                await _service.Delete(id);
                TempData["Success"] = $"A questão \"{questao.Texto}\" foi excluída com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Não foi possível excluir a questão, tente novamente. Detalhes do erro: {ex.Message}";
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
