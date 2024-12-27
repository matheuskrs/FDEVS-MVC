using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using FDevs.Services.UsuarioService;
using FDevs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers
{


    [Authorize]
    public class ProvasUsuarioController : Controller
    {
        private readonly ILogger<ProvasUsuarioController> _logger;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IUsuarioService _userService;

        public ProvasUsuarioController(ILogger<ProvasUsuarioController> logger, AppDbContext context, IWebHostEnvironment host, IUsuarioService userService)
        {
            _logger = logger;
            _context = context;
            _host = host;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Respostas(int id)
        {
            var currentUser = await _userService.GetUsuarioLogado();
            ViewBag.User = currentUser;
            var prova = await _context.Provas
                .Include(p => p.Curso)
                .Include(p => p.Questoes)
                .ThenInclude(q => q.Alternativas)
                .SingleOrDefaultAsync(p => p.Id == id);

            var respostas = await _context.Respostas
                .Where(r => r.UsuarioId == currentUser.UsuarioId && r.Questao.ProvaId == id)
                .Include(r => r.Questao)
                .Include(r => r.Alternativa)
                .ToListAsync();

            var questoes = await _context.Questoes.ToListAsync();

            var provaVM = new ProvaVM
            {
                Prova = prova,
                Respostas = respostas,
                Questoes = questoes
            };
            return View(provaVM);
        }


        [HttpGet]
        public async Task<IActionResult> Index(int id, int? questaoId)
        {
            var currentUser = await _userService.GetUsuarioLogado();
            ViewBag.User = currentUser;

            var prova = await _context.Provas
                .Include(p => p.Curso)
                .Include(p => p.Questoes)
                .ThenInclude(q => q.Alternativas)
                .SingleOrDefaultAsync(p => p.Id == id);

            var totalQuestoes = prova.Questoes.Count();
            var respostasUsuario = await _context.Respostas
                .Where(r => r.UsuarioId == currentUser.UsuarioId && r.Questao.ProvaId == id)
                .ToListAsync();

            if (respostasUsuario.Count() == totalQuestoes)
            {
                return RedirectToAction("Respostas", new { id });
            }

            var questaoAtualId = questaoId ?? prova.Questoes.FirstOrDefault()?.Id;

            var questaoAtual = await _context.Questoes
                .Include(q => q.Alternativas)
                .Include(q => q.Respostas)
                .FirstOrDefaultAsync(q => q.Id == questaoAtualId);

            var alternativas = questaoAtual.Alternativas.ToList();

            var proximaQuestao = prova.Questoes
              .OrderBy(q => q.Id)
              .Where(q => q.ProvaId == questaoAtual.ProvaId)
              .FirstOrDefault(q => q.Id > questaoAtualId);

            var questaoAnterior = prova.Questoes
                .Where(q => q.ProvaId == questaoAtual.ProvaId)
                .OrderByDescending(q => q.Id)
                .FirstOrDefault(q => q.Id < questaoAtualId);

            var resposta = await _context.Respostas.FirstOrDefaultAsync(r => r.QuestaoId == questaoAtualId && r.UsuarioId == currentUser.UsuarioId);

            var provaVM = new ProvaVM
            {
                QuestaoId = questaoAtualId,
                Questoes = prova.Questoes.ToList(),
                Alternativas = alternativas,
                ProximaQuestao = proximaQuestao,
                QuestaoAnterior = questaoAnterior,
                QuestaoAtual = questaoAtual,
                Resposta = resposta,
                Prova = prova
            };
            return View(provaVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Resposta resposta)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userService.GetUsuarioLogado();
            ViewBag.User = currentUser;

            var questaoAtual = await _context.Questoes
                .Include(q => q.Alternativas)
                .Include(q => q.Respostas)
                .FirstOrDefaultAsync(q => q.Id == resposta.QuestaoId);

            var respostaExistente = await _context.Respostas
                .FirstOrDefaultAsync(r => r.UsuarioId == resposta.UsuarioId && r.QuestaoId == resposta.QuestaoId);

            var prova = await _context.Provas
                .Include(p => p.Curso)
                .Include(p => p.Questoes)
                .ThenInclude(q => q.Alternativas)
                .SingleOrDefaultAsync(p => p.Id == questaoAtual.ProvaId);

            var totalQuestoes = prova.Questoes.Count();
            var respostasUsuario = await _context.Respostas
                .Where(r => r.UsuarioId == currentUser.UsuarioId && r.Questao.ProvaId == questaoAtual.ProvaId)
                .ToListAsync();

            if (respostasUsuario.Count() == totalQuestoes)
            {
                return RedirectToAction("Respostas", new { questaoAtual.ProvaId });
            }


            if (ModelState.IsValid)
            {
                if (respostaExistente != null)
                {
                    respostaExistente.AlternativaId = resposta.AlternativaId;
                    _context.Update(respostaExistente);
                    resposta.Alternativa = await _context.Alternativas.FirstOrDefaultAsync(r => r.Id == resposta.AlternativaId);
                    resposta.Questao = questaoAtual;
                }
                else
                {
                    _context.Add(resposta);
                    resposta.Questao = questaoAtual;
                }

                if (resposta.Alternativa == null)
                {
                    return RedirectToAction("Index", "ProvasUsuario", new { id = resposta.Questao.ProvaId, questaoId = questaoAtual.Id });
                }

                var proximaQuestao = await _context.Questoes
                    .Where(q => q.Id > resposta.QuestaoId)
                    .Where(q => q.ProvaId == resposta.Questao.ProvaId)
                    .OrderBy(q => q.Id)
                    .FirstOrDefaultAsync();

                await _context.SaveChangesAsync();

                if (proximaQuestao != null)
                {
                    return RedirectToAction("Index", "ProvasUsuario", new { id = resposta.Questao.ProvaId, questaoId = proximaQuestao.Id });
                }
            }
            return RedirectToAction("Respostas", new { id = resposta.Questao.ProvaId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}