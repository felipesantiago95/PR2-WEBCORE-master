using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PR2_WEBCORE.Models;
using PR2_WEBCORE.Servicios.Contrato;
using System.Linq;
using System.Threading.Tasks;

namespace PR2_WEBCORE.Controllers
{
    [Authorize(Roles = "Administrador,Asesor,Supervisor")]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IUsuarioService _usuarioService;
        private readonly IEstablecimientoService _establecimientoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IAforoService _aforoService;
        private readonly IClaseService _claseService;

        public SaleController(ISaleService saleService,
                              IUsuarioService usuarioService,
                              IEstablecimientoService establecimientoService,
                              ICategoriaService categoriaService,
                              IAforoService aforoService,
                              IClaseService claseService)
        {
            _saleService = saleService;
            _usuarioService = usuarioService;
            _establecimientoService = establecimientoService;
            _categoriaService = categoriaService;
            _aforoService = aforoService;
            _claseService = claseService;
        }

        // GET: Sale
        public async Task<IActionResult> Index()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return View(sales);
        }

        // GET: Sale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await LoadViewBags();
            var sale = await _saleService.GetSaleByIdAsync(id.Value);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sale/Create
        public async Task<IActionResult> Create()
        {
            await LoadViewBags();
            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSales,IdUsuario,IdEstablecimiento,IdCategoria,IdAforo,IdClase,Total")] Sale sale)
        {
            
                var aforo = await _aforoService.GetAforoById((int)sale.IdAforo);
                var categoria = await _categoriaService.GetCategoriaByIdAsync((int)sale.IdCategoria);
                var clase = await _claseService.GetByIdAsync((int)sale.IdClase);

                if (aforo == null || categoria == null || clase == null)
                {
                    ModelState.AddModelError("", "No se pudo obtener la información necesaria para calcular el total.");
                    await LoadViewBags(sale);
                    return View(sale);
                }

                decimal baseValue = 460;
                decimal aforoValue = baseValue * ((decimal)aforo.Porcentaje);
                decimal categoriaValue = baseValue * ((decimal)categoria.Porcentaje);
                decimal claseValue = baseValue * ((decimal)clase.Porcentaje);

                sale.Total = baseValue + aforoValue + categoriaValue + claseValue;

                await _saleService.CreateSaleAsync(sale);
               return RedirectToAction(nameof(Index));
            

            await LoadViewBags(sale);
            return View(sale);
        }

        // GET: Sale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _saleService.GetSaleByIdAsync(id.Value);
            if (sale == null)
            {
                return NotFound();
            }

            await LoadViewBags(sale);
            return View(sale);
        }

        // POST: Sale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdSales,IdAforo,IdCategoria,IdClase,IdEstablecimiento,IdUsuario,Total,RowVersion,FechaCreacion")] Sale sale)
        {
            
                try
                {
                    var aforo = await _aforoService.GetAforoById((int)sale.IdAforo);
                    var categoria = await _categoriaService.GetCategoriaByIdAsync((int)sale.IdCategoria);
                    var clase = await _claseService.GetByIdAsync((int)sale.IdClase);

                    if (aforo == null || categoria == null || clase == null)
                    {
                        ModelState.AddModelError("", "No se pudo obtener la información necesaria para calcular el total.");
                        await LoadViewBags(sale);
                        return View(sale);
                    }

                    decimal baseValue = 460;
                    decimal aforoValue = baseValue * ((decimal)aforo.Porcentaje);
                    decimal categoriaValue = baseValue * ((decimal)categoria.Porcentaje);
                    decimal claseValue = baseValue * ((decimal)clase.Porcentaje);

                    sale.Total = baseValue + aforoValue + categoriaValue + claseValue;
                    await _saleService.UpdateSaleAsync(sale);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty, "La venta fue modificada por otro usuario. Por favor, intente de nuevo.");
                }
            
            return View(sale);
        }

        // GET: Sale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await LoadViewBags();
            var sale = await _saleService.GetSaleByIdAsync(id.Value);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sale/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _saleService.DeleteSaleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SaleExists(int id)
        {
            var valid = await _saleService.GetSaleByIdAsync(id);
            return valid != null;
        }

        private async Task LoadViewBags(Sale sale = null)
        {
            ViewBag.Usuarios = (await _usuarioService.GetAllUsuarios())
                .Select(u => new SelectListItem { Value = u.IdUsuario.ToString(), Text = u.Nombre }).ToList();
            ViewBag.Establecimientos = (await _establecimientoService.GetAllEstablecimientos())
                .Select(e => new SelectListItem { Value = e.IdEstablecimiento.ToString(), Text = e.RazonSocial }).ToList();
            ViewBag.Categorias = (await _categoriaService.GetAllCategoriasAsync())
                .Select(c => new SelectListItem { Value = c.IdCagegoria.ToString(), Text = c.Descripcion }).ToList();
            ViewBag.Aforos = (await _aforoService.GetAllAforos())
                .Select(a => new SelectListItem { Value = a.IdAforo.ToString(), Text = a.Descripcion }).ToList();
            ViewBag.Clases = (await _claseService.GetAllAsync())
                .Select(c => new SelectListItem { Value = c.IdClase.ToString(), Text = c.Descripcion }).ToList();

            if (sale != null)
            {
                ViewBag.SelectedUsuario = sale.IdUsuario;
                ViewBag.SelectedEstablecimiento = sale.IdEstablecimiento;
                ViewBag.SelectedCategoria = sale.IdCategoria;
                ViewBag.SelectedAforo = sale.IdAforo;
                ViewBag.SelectedClase = sale.IdClase;
            }
        }
    }
}
