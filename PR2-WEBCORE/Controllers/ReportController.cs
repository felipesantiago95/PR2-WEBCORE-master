using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PR2_WEBCORE.Models;
using PR2_WEBCORE.Services;
using PR2_WEBCORE.Servicios.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR2_WEBCORE.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IUsuarioService _usuarioService;
        private readonly IEstablecimientoService _establecimientoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IAforoService _aforoService;
        private readonly IClaseService _claseService;

        public ReportController(
            IReportService reportService,
            IUsuarioService usuarioService,
            IEstablecimientoService establecimientoService,
            ICategoriaService categoriaService,
            IAforoService aforoService,
            IClaseService claseService)
        {
            _reportService = reportService;
            _usuarioService = usuarioService;
            _establecimientoService = establecimientoService;
            _categoriaService = categoriaService;
            _aforoService = aforoService;
            _claseService = claseService;
        }

        [HttpGet]
        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate, int? idUsuario, int? idEstablecimiento, int? idCategoria, int? idAforo, int? idClase)
        {
            var salesReport = await _reportService.GetSalesReportAsync(startDate, endDate, idUsuario, idEstablecimiento, idCategoria, idAforo, idClase);

            ViewBag.Users = (await _usuarioService.GetAllAsync()).Select(u => new SelectListItem { Value = u.IdUsuario.ToString(), Text = u.Nombre }).ToList();
            ViewBag.Establecimientos = (await _establecimientoService.GetAllAsync()).Select(e => new SelectListItem { Value = e.IdEstablecimiento.ToString(), Text = e.RazonSocial }).ToList();
            ViewBag.Categorias = (await _categoriaService.GetAllAsync()).Select(c => new SelectListItem { Value = c.IdCagegoria.ToString(), Text = c.Descripcion }).ToList();
            ViewBag.Aforos = (await _aforoService.GetAllAsync()).Select(a => new SelectListItem { Value = a.IdAforo.ToString(), Text = a.Descripcion }).ToList();
            ViewBag.Clases = (await _claseService.GetAllAsync()).Select(c => new SelectListItem { Value = c.IdClase.ToString(), Text = c.Descripcion }).ToList();

            // Calcular el total de las ventas
            decimal totalSales = salesReport.Sum(s => (decimal)s.Total);

            ViewBag.TotalSales = totalSales;

            // Preparar datos para el gráfico (puedes implementar esta parte según la biblioteca de gráficos que prefieras utilizar)
            // Aquí un ejemplo básico utilizando ViewBag para datos de prueba
            ViewBag.ChartData = new List<int> { salesReport.Count(s => s.IdCategoria == idCategoria), salesReport.Count(s => s.IdAforo == idAforo), salesReport.Count(s => s.IdClase == idClase) };

            return View(salesReport);
        }
    }
}
