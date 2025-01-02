using FDevs.Data;
using FDevs.Models;
using FDevs.Services.EstadoService;
using FDevs.Services.ExclusaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EstadosController : Controller
    {
        private readonly IEstadoService _service;
        private readonly IExclusaoService _deleteService;

        public EstadosController(IEstadoService service, IExclusaoService deleteService)
        {
            _service = service;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Estado> estados = await _service.GetEstadosAsync();
            return View(estados);
        }

        public async Task<IActionResult> Details(int id)
        {
            Estado estado = await _service.GetEstadoByIdAsync(id);
            if (estado == null) return RedirectToAction("Index");
            return View(estado);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estado estado)
        {
            if (!ModelState.IsValid) return View(estado);
            try
            {
                await _service.Create(estado);
                TempData["Success"] = $"O estado \"{estado.Nome}\" foi criado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao tentar criar o estado, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var estado = await _service.GetEstadoByIdAsync(id);
            if (estado == null) return RedirectToAction("Index");
            return View(estado);
        }

        public async Task<IActionResult> EditConfirmed(Estado estado)
        {
            if (!ModelState.IsValid) return View(estado);

            try
            {
                await _service.Update(estado);
                TempData["Success"] = $"O estado \"{estado.Nome}\" foi alterado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao tentar alterar o estado, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var estado = await _service.GetEstadoByIdAsync(id);
            if (estado == null) return RedirectToAction("Index");
            return View(estado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var estado = await _service.GetEstadoByIdAsync(id);
            if (estado == null) return RedirectToAction("Index");
            string mensagemErro = _deleteService.PermitirExcluirEstado(estado);

            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction("Index");
            }
            try
            {
                await _service.Delete(estado.Id);
                TempData["Success"] = $"O estado \"{estado.Nome}\" foi excluído com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"O estado não pode ser excluído, tente novamente. Detalhes do erro: {ex.Message}";
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