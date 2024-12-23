using FDevs.Models;

namespace FDevs.ViewModels
{


    public class AcertoVM
    {
        public Usuario Usuario { get; set; }
        public Prova Prova { get; set; }
        public int QuantidadeAcertos { get; set; }
        public int TotalQuestoes { get; set; }
    }
}