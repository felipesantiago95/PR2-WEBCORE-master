using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PR2_WEBCORE.Models;
using System.Collections.Generic;

namespace PR2_WEBCORE.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class DashboardController : Controller
    {
        private readonly IngwebCoreContext _context;

        public DashboardController(IngwebCoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Método para obtener datos de ventas para los gráficos
        public async Task<IActionResult> SalesData()
        {
            var salesData = await _context.Sales
                .Include(s => s.IdCategoriaNavigation)
                .Include(s => s.IdAforoNavigation)
                .Include(s => s.IdClaseNavigation)
                .ToListAsync();

            return Json(salesData);
        }
    }
}
