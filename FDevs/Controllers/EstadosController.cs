using FDevs.Data;
using FDevs.Models;
using FDevs.Services.EstadoService;
using FDevs.Services.ExclusaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{



    [Authorize(Roles = "Administrador")]
    public class EstadosController : Controller
    {
        private readonly ILogger<EstadosController> _logger;
        private readonly AppDbContext _context;
        private readonly IEstadoService _service;
        private readonly IExclusaoService _deleteService;

        public EstadosController(ILogger<EstadosController> logger, AppDbContext context, IEstadoService service, IExclusaoService deleteService)
        {
            _logger = logger;
            _context = context;
            _service = service;
            _deleteService = deleteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var estados = await _service.GetEstadosAsync();
            return View(estados);
        }

        public async Task<IActionResult> Details(int id)
        {
            var estado = await _service.GetEstadoByIdAsync(id);
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
            if (ModelState.IsValid)
            {
                await _service.Create(estado);
                TempData["Success"] = $"O estado '{estado.Nome}' foi criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(estado);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var estado = await _service.GetEstadoByIdAsync(id);
            return View(estado);
        }

        public async Task<IActionResult> EditConfirmed(Estado estado)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(estado);
                TempData["Success"] = $"O estado '{estado.Nome}' foi alterado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(estado);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var estado = await _service.GetEstadoByIdAsync(id);
            return View(estado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var estado = await _service.GetEstadoByIdAsync(id);
            if (estado == null) return NotFound();
            string mensagemErro = _deleteService.PermitirExcluirEstado(estado);

            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction(nameof(Index));
            }

            await _service.Delete(estado.Id);
            TempData["Success"] = $"O estado '{estado.Nome}' foi excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}