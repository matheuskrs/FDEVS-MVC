using FDevs.Data;
using FDevs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{



    [Authorize(Roles = "Administrador")]
    public class RespostasController : Controller
    {
        private readonly ILogger<RespostasController> _logger;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;

        public RespostasController(ILogger<RespostasController> logger, AppDbContext context, IWebHostEnvironment host)
        {
            _logger = logger;
            _context = context;
            _host = host;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var respostas = await _context.Respostas
                .Include(r => r.Usuario)
                .Include(r => r.Questao)
                .ThenInclude(q => q.Prova)
                .Include(r => r.Alternativa)
                .ToListAsync();
            return View(respostas);
        }

        public async Task<IActionResult> Details(int id)
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nome");
            ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
            ViewData["AlternativaId"] = new SelectList(_context.Alternativas, "Id", "Texto");
            var resposta = await _context.Respostas.SingleOrDefaultAsync(r => r.Id == id);
            return View(resposta);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Respostas == null)
            {
                return NotFound();
            }
            var resposta = await _context.Respostas
                .SingleOrDefaultAsync(r => r.Id == id);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nome");
            ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
            ViewData["AlternativaId"] = new SelectList(_context.Alternativas.Where(a => a.QuestaoId == resposta.QuestaoId), "Id", "Texto");
            return View(resposta);
        }

        public async Task<IActionResult> EditConfirmed(int id, Resposta resposta)
        {
            if (id != resposta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(resposta);
                await _context.SaveChangesAsync();
                resposta = await _context.Respostas
                    .Include(r => r.Usuario)
                    .SingleOrDefaultAsync(
                        r => r.UsuarioId == resposta.UsuarioId &&
                        r.QuestaoId == resposta.QuestaoId
                    );
                TempData["Success"] = $"A resposta de {resposta.Usuario.Nome} foi alterada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View("Edit", resposta);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nome");
            ViewData["QuestaoId"] = new SelectList(_context.Questoes, "Id", "Texto");
            ViewData["AlternativaId"] = new SelectList(_context.Alternativas, "Id", "Texto");
            var resposta = await _context.Respostas.SingleOrDefaultAsync(r => r.Id == id);
            if (resposta == null)
            {
                return NotFound();
            }
            return View(resposta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var resposta = await _context.Respostas.SingleOrDefaultAsync(r => r.Id == id);
            if (resposta == null)
                return NotFound();

            _context.Remove(resposta);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"A Resposta foi excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}