using PR2_WEBCORE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PR2_WEBCORE.Servicios.Contrato
{
    public interface IUsuarioRolService
    {
        Task<List<UsuarioRol>> GetAllUsuarioRolesAsync();
        //IAsyncEnumerable<UsuarioRol> GetUsuarioRolByIdAsync(int id);
        Task<UsuarioRol> GetUsuarioRolByIdAsync(int id);
        Task SaveUsuarioRolAsync(UsuarioRol usuarioRol);
        Task UpdateUsuarioRolAsync(UsuarioRol usuarioRol);
        Task DeleteUsuarioRolAsync(int id);
    }
}
