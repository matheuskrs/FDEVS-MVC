

using System.Security.Claims;
using FDevs.Data;
using FDevs.Models;
using FDevs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Controllers;

public class ProvasUsuarioController : Controller
{
    private readonly ILogger<ProvasUsuarioController> _logger;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public ProvasUsuarioController(ILogger<ProvasUsuarioController> logger, AppDbContext context, IWebHostEnvironment host)
    {
        _logger = logger;
        _context = context;
        _host = host;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id, int? questaoId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;

        var cursos = _context.Cursos.ToList();

        var prova = await _context.Provas
            .Include(p => p.Curso)
            .Include(p => p.Questoes)
            .ThenInclude(p => p.Alternativas)
            .SingleOrDefaultAsync(p => p.Id == id);

        var questaoAtualId = questaoId ?? prova.Questoes.FirstOrDefault()?.Id;
        var questaoAtual = _context.Questoes
            .Include(q => q.Alternativas)
            .Include(q => q.Respostas)
            .FirstOrDefault(q => q.Id == questaoAtualId);
        var alternativas = questaoAtual.Alternativas.ToList();
        var respostas = _context.Respostas.ToList();
        // Buscar a próxima questão
        var proximaQuestao = prova.Questoes
          .OrderBy(q => q.Id)
          .Where(q => q.ProvaId == questaoAtual.ProvaId)
          .FirstOrDefault(q => q.Id > questaoAtualId);

        var questaoAnterior = prova.Questoes
            .Where(q => q.ProvaId == questaoAtual.ProvaId)
            .OrderByDescending(q => q.Id)
            .FirstOrDefault(q => q.Id < questaoAtualId);
        var provaVM = new ProvaVM
        {
            QuestaoId = questaoAtualId,
            Questoes = prova.Questoes.ToList(),
            Alternativas = alternativas,
            ProximaQuestao = proximaQuestao,
            QuestaoAnterior = questaoAnterior,
            QuestaoAtual = questaoAtual
        };
        return View(provaVM);
    }

    [HttpPost]
    public IActionResult Create(Resposta resposta)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == currentUserId);
        ViewBag.User = currentUser;
        resposta.UsuarioId = ViewBag.User.UsuarioId;
        resposta.QuestaoId = resposta.QuestaoId;
        _context.Respostas.Add(resposta);
        var proximaQuestao = _context.Questoes
            .Where(q => q.Id > resposta.QuestaoId)
            .OrderBy(q => q.Id)
            .FirstOrDefault();
        var questaoAtual = _context.Questoes
            .Include(q => q.Alternativas)
            .Include(q => q.Respostas)
            .FirstOrDefault(q => q.Id == resposta.QuestaoId);
        resposta.Questao = questaoAtual;
        _context.SaveChanges();
        if (proximaQuestao != null)
        {
            return RedirectToAction("Index", new { provaId = resposta.Questao.ProvaId, questaoId = proximaQuestao.Id });
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}