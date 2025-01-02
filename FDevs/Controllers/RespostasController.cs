using FDevs.Data;
using FDevs.Models;
using FDevs.Services.RespostaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RespostasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRespostaService _service;

        public RespostasController(AppDbContext context, IRespostaService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Resposta> respostas = await _service.GetRespostasAsync();
            return View(respostas);
        }

        public async Task<IActionResult> Details(int id)
        {
            Resposta resposta = await _service.GetRespostaByIdAsync(id);
            if (resposta == null) return RedirectToAction("Index");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nome");
            ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
            ViewData["AlternativaId"] = new SelectList(_context.Alternativas, "Id", "Texto");
            return View(resposta);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Resposta resposta = await _service.GetRespostaByIdAsync(id);
            if (resposta == null) return RedirectToAction("Index");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nome");
            ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
            ViewData["AlternativaId"] = new SelectList(_context.Alternativas.Where(a => a.QuestaoId == resposta.QuestaoId), "Id", "Texto");
            return View(resposta);
        }

        public async Task<IActionResult> EditConfirmed(int id, Resposta resposta)
        {
            if (id != resposta.Id) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View(resposta);
            try
            {
                await _service.Update(resposta);
                resposta = await _service.GetRespostaByIdAsync(id);
                TempData["Success"] = $"A resposta de \"{resposta.Usuario.Nome}\" foi alterada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao tentar alterar a resposta, tente novamente. Detalhes do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Resposta resposta = await _service.GetRespostaByIdAsync(id);
            if (resposta == null) return RedirectToAction("Index");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nome");
            ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
            ViewData["AlternativaId"] = new SelectList(_context.Alternativas, "Id", "Texto");
            return View(resposta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Resposta resposta = await _service.GetRespostaByIdAsync(id);
            if (resposta == null) return RedirectToAction("Index");
            try
            {
                _context.Remove(resposta);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"A resposta foi excluída com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Ocorreu um erro ao excluir a resposta, tente novamente. Detalhes do erro: {ex.Message}";
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