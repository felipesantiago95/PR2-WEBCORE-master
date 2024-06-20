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
    [Authorize(Roles = "Administrador")]
    public class UsuarioRolAdminController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRoleService _roleService;
        private readonly IUsuarioRolService _usuarioRolService;

        public UsuarioRolAdminController(IUsuarioService usuarioService, IRoleService roleService, IUsuarioRolService usuarioRolService)
        {
            _usuarioService = usuarioService;
            _roleService = roleService;
            _usuarioRolService = usuarioRolService;
        }
        private async Task LoadViewBagAsync()
        {
            var roles = await _roleService.GetAllRolesAsync();
            var usuarios = await _usuarioService.GetAllUsuarios();

            ViewBag.Roles = roles;
            ViewBag.Usuarios = usuarios;
        }

        // GET: UsuarioRol
        public async Task<IActionResult> Index()
        {
            var usuarioRols = await _usuarioRolService.GetAllUsuarioRolesAsync();
            var usuarios = await _usuarioService.GetAllUsuarios();
            var roles = await _roleService.GetAllRolesAsync();

            var viewModel = from ur in usuarioRols
                            join u in usuarios on ur.IdUsuario equals u.IdUsuario
                            join r in roles on ur.IdRoles equals r.IdRol
                            select new
                            {
                                ur.IdUserRoles,
                                Usuario = u.Nombre,
                                DescripcionRol = r.Descripcion
                            };

            return View(viewModel.ToList());
        }


        public async Task<IActionResult> Create()
        {
            await LoadViewBagAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioRol usuarioRol)
        {

            await _usuarioRolService.SaveUsuarioRolAsync(usuarioRol);
            return RedirectToAction(nameof(Index));

            await LoadViewBagAsync();
            return View(usuarioRol);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuarioRol = await _usuarioRolService.GetUsuarioRolByIdAsync(id);
            if (usuarioRol == null)
            {
                return NotFound();
            }
            await LoadViewBagAsync();
            return View(usuarioRol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUserRoles,IdUsuario,IdRoles")] UsuarioRol usuarioRol)
        {
            usuarioRol.IdUserRoles = id;
            try
            {
                await _usuarioRolService.UpdateUsuarioRolAsync(usuarioRol);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UsuarioRolExists(usuarioRol.IdUserRoles))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            await LoadViewBagAsync();
            return View(usuarioRol);
        }

        private async Task<bool> UsuarioRolExists(int id)
        {
            return await _usuarioRolService.GetUsuarioRolByIdAsync(id) != null;
        }


        public async Task<IActionResult> Delete(int id)
        {
            var usuarioRol = await _usuarioRolService.GetUsuarioRolByIdAsync(id);
            if (usuarioRol == null)
            {
                return NotFound();
            }
            return View(usuarioRol);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete2(int id)
        {
            await _usuarioRolService.DeleteUsuarioRolAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
