using Microsoft.EntityFrameworkCore;
using PR2_WEBCORE.Models;
using PR2_WEBCORE.Servicios.Contrato;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR2_WEBCORE.Servicios.Implementacion
{
    public class UsuarioRolService : IUsuarioRolService
    {
        private readonly IngwebCoreContext _dbContext;

        public UsuarioRolService(IngwebCoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<UsuarioRol>> GetAllUsuarioRolesAsync()
        {
            return await _dbContext.UsuarioRols
                .Include(ur => ur.IdUsuarioNavigation)
                .Include(ur => ur.IdRolesNavigation)
                .ToListAsync();
        }
        public async Task<UsuarioRol> GetUsuarioRolByIdAsync(int id)
        {
            return await _dbContext.UsuarioRols
                .Include(ur => ur.IdUsuarioNavigation)
                .Include(ur => ur.IdRolesNavigation)
                .FirstOrDefaultAsync(ur => ur.IdUserRoles == id);
        }


        public async Task SaveUsuarioRolAsync(UsuarioRol usuarioRol)
        {
            _dbContext.UsuarioRols.Add(usuarioRol);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUsuarioRolAsync(UsuarioRol usuarioRol)
        {
            _dbContext.Entry(usuarioRol).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUsuarioRolAsync(int id)
        {
            var usuarioRol = await _dbContext.UsuarioRols.FindAsync(id);
            if (usuarioRol != null)
            {
                _dbContext.UsuarioRols.Remove(usuarioRol);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
