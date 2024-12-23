
using FDevs.Models;

namespace FDevs.ViewModels
{


    public class ProvaVM
    {
        public int? QuestaoId { get; set; }
        public List<Alternativa> Alternativas { get; set; }
        public List<Questao> Questoes { get; set; }
        public Resposta Resposta { get; set; }
        public List<Resposta> Respostas { get; set; }
        public Prova Prova { get; set; }
        public Questao ProximaQuestao { get; set; }
        public Questao QuestaoAtual { get; set; }
        public Questao QuestaoAnterior { get; set; }
    }
}