using Microsoft.EntityFrameworkCore;
using PR2_WEBCORE.Models;
using PR2_WEBCORE.Servicios.Contrato;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR2_WEBCORE.Servicios.Implementacion
{
    public class SaleService : ISaleService
    {
        private readonly IngwebCoreContext _dbContext;

        public SaleService(IngwebCoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _dbContext.Sales
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdEstablecimientoNavigation)
                .Include(s => s.IdCategoriaNavigation)
                .Include(s => s.IdAforoNavigation)
                .Include(s => s.IdClaseNavigation)
                .ToListAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            return await _dbContext.Sales.FindAsync(id);
        }

        public async Task<int> CreateSaleAsync(Sale sale)
        {
            sale.FechaCreacion = DateTime.Now; // Establecer la fecha de creación
            _dbContext.Sales.Add(sale);
            await _dbContext.SaveChangesAsync();
            return sale.IdSales;
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            var existingSale = await _dbContext.Sales.FindAsync(sale.IdSales);
            _dbContext.Entry(existingSale).CurrentValues.SetValues(sale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSaleAsync(int id)
        {
            var sale = await _dbContext.Sales.FindAsync(id);
            if (sale != null)
            {
                _dbContext.Sales.Remove(sale);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
