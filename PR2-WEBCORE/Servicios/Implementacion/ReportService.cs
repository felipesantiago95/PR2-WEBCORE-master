using Microsoft.EntityFrameworkCore;
using PR2_WEBCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR2_WEBCORE.Services
{
    public class ReportService : IReportService
    {
        private readonly IngwebCoreContext _context;

        public ReportService(IngwebCoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetSalesReportAsync(DateTime? startDate, DateTime? endDate, int? idUsuario, int? idEstablecimiento, int? idCategoria, int? idAforo, int? idClase)
        {
            var query = _context.Sales
                .Include(s => s.IdAforoNavigation)
                .Include(s => s.IdCategoriaNavigation)
                .Include(s => s.IdClaseNavigation)
                .Include(s => s.IdEstablecimientoNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(s => s.FechaCreacion >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(s => s.FechaCreacion <= endDate.Value);
            }

            if (idUsuario.HasValue)
            {
                query = query.Where(s => s.IdUsuario == idUsuario.Value);
            }

            if (idEstablecimiento.HasValue)
            {
                query = query.Where(s => s.IdEstablecimiento == idEstablecimiento.Value);
            }

            if (idCategoria.HasValue)
            {
                query = query.Where(s => s.IdCategoria == idCategoria.Value);
            }

            if (idAforo.HasValue)
            {
                query = query.Where(s => s.IdAforo == idAforo.Value);
            }

            if (idClase.HasValue)
            {
                query = query.Where(s => s.IdClase == idClase.Value);
            }

            return await query.ToListAsync();
        }
    }
}
