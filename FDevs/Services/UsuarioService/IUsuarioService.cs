using FDevs.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace FDevs.Services.UsuarioService
{
    public interface IUsuarioService
    {
        Task<UsuarioVM> GetUsuarioLogado();
        Task<SignInResult> LoginUsuario(LoginVM login);
        Task LogoffUsuario();
        Task<List<string>> RegistrarUsuario(RegistroVM registro);
    }
}