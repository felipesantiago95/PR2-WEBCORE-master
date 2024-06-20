using PR2_WEBCORE.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PR2_WEBCORE.Services
{
    public interface IReportService
    {
        Task<IEnumerable<Sale>> GetSalesReportAsync(DateTime? startDate, DateTime? endDate, int? idUsuario, int? idEstablecimiento, int? idCategoria, int? idAforo, int? idClase);
    }
}
