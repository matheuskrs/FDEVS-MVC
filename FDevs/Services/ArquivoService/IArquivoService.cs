namespace FDevs.Services.ArquivoService
{
    public interface IArquivoService
    {
        Task<string> SalvarArquivoAsync(IFormFile file, string folderPath, string fileName);
    }
}
