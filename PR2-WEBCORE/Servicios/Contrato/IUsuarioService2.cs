using PR2_WEBCORE.Models;

namespace PR2_WEBCORE.Servicios.Contrato
{
    public interface IUsuarioService2
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> GetByIdAsync(int id);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
    }
}
