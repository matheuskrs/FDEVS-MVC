using FDevs.Models;
using FDevs.Services.ArquivoService;
using FDevs.Services.ExclusaoService;
using FDevs.Services.TrilhaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TrilhasController : Controller
    {
        private readonly ITrilhaService _service;
        private readonly IExclusaoService _deleteService;
        private readonly IArquivoService _arquivoService;

        public TrilhasController(ITrilhaService service, IExclusaoService deleteService, IArquivoService arquivoService)
        {
            _service = service;
            _deleteService = deleteService;
            _arquivoService = arquivoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Trilha> trilhas = await _service.GetTrilhasAsync();
            return View(trilhas);
        }

        public async Task<IActionResult> Details(int id)
        {
            Trilha trilha = await _service.GetTrilhaByIdAsync(id);
            if (trilha == null) return RedirectToAction("Index");
            return View(trilha);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trilha trilha, IFormFile Arquivo)
        {
            if (!ModelState.IsValid) return View(trilha);
            try
            {
                if (Arquivo != null)
                {
                    string fileName = trilha.Id + Path.GetExtension(Arquivo.FileName);
                    string caminho = "img\\Trilhas";
                    trilha.Foto = await _arquivoService.SalvarArquivoAsync(Arquivo, caminho, fileName);
                }
                else
                {
                    trilha.Foto = "\\img\\Trilhas\\sem-foto.png";
                }
                await _service.Create(trilha);
                TempData["Success"] = $"A trilha \"{trilha.Nome}\" foi criada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao criar a trilha, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            Trilha trilha = await _service.GetTrilhaByIdAsync(id);
            if (trilha == null) return RedirectToAction("Index");
            return View(trilha);
        }

        public async Task<IActionResult> EditConfirmed(Trilha trilha, IFormFile Arquivo)
        {
            Trilha trilhaExistente = await _service.GetTrilhaAsNoTracking(trilha.Id);

            if (!ModelState.IsValid) return View(trilha);

            try
            {
                if (Arquivo != null)
                {
                    string fileName = trilha.Id + Path.GetExtension(Arquivo.FileName);
                    string caminho = "img\\Trilhas";
                    trilha.Foto = await _arquivoService.SalvarArquivoAsync(Arquivo, caminho, fileName);
                }
                else
                {
                    trilha.Foto = trilhaExistente.Foto;
                }

                await _service.Update(trilha);
                TempData["Success"] = $"A trilha \"{trilha.Nome}\" foi alterada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao alterar a trilha, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trilha = await _service.GetTrilhaByIdAsync(id);
            if (trilha == null) return RedirectToAction("Index");
            return View(trilha);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Trilha trilha = await _service.GetTrilhaByIdAsync(id);
            if (trilha == null) return NoContent();

            string mensagemErro = _deleteService.PermitirExcluirTrilha(trilha);

            if (mensagemErro != null)
            {
                TempData["Warning"] = mensagemErro;
                return RedirectToAction("Index");
            }
            try
            {
                await _service.Delete(id);
                TempData["Success"] = $"A trilha \"{trilha.Nome}\" foi excluída com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao deletar a trilha, tente novamente. Detalhes do erro: {ex.Message}";
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