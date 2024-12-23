
namespace FDevs.Services.ArquivoService
{
    public class ArquivoService : IArquivoService
    {
        private readonly IWebHostEnvironment _host;

        public ArquivoService(IWebHostEnvironment host)
        {
            _host = host;
        }

        public async Task<string> SalvarArquivoAsync(IFormFile Arquivo, string caminho, string nomeDoArquivo)
        {
            string caminhoDaPasta = Path.Combine(_host.WebRootPath, caminho);
            string caminhoCompleto = Path.Combine(caminhoDaPasta, nomeDoArquivo);

            if (!Directory.Exists(caminhoDaPasta)) Directory.CreateDirectory(caminhoDaPasta);
            
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await Arquivo.CopyToAsync(stream);
            }
            return Path.Combine("\\" + caminho + "\\" + nomeDoArquivo);
        }
    }
}
