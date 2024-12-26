using FDevs.Data;
using FDevs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AcertosController : Controller
    {
        private readonly AppDbContext _context;

        public AcertosController(AppDbContext context)
        {
            _context = context;
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

            var acertos = respostas
                .GroupBy(r => new { r.Usuario, r.Questao.Prova })
                .Select(acertoVM => new AcertoVM
                {
                    Usuario = acertoVM.Key.Usuario,
                    Prova = acertoVM.Key.Prova,
                    QuantidadeAcertos = acertoVM.Count(r => r.Alternativa.Correta),
                    TotalQuestoes = acertoVM.Count()
                })
                .ToList();

            return View(acertos);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

}