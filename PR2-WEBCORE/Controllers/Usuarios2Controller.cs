using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR2_WEBCORE.Models;
using PR2_WEBCORE.Servicios.Contrato;
using System.Threading.Tasks;
using PR2_WEBCORE.Recursos;
using Microsoft.AspNetCore.Authorization;

namespace PR2_WEBCORE.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class Usuarios2Controller : Controller
    {
        private readonly IUsuarioService2 _usuarioService;
        private readonly IUsuarioRolService _usuarioRolService;

        public Usuarios2Controller(IUsuarioService2 usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: Usuarios2
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return View(usuarios);
        }

        // GET: Usuarios2/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios2/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios2/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Apellido,FechaCreacion,Correo,Clave")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaCreacion = DateTime.Now;
				await _usuarioService.AddAsync(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios2/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios2/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Apellido,FechaCreacion,Correo,Clave")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Clave = Utilidades.EncriptarClave(usuario.Clave);
                    await _usuarioService.UpdateAsync(usuario);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _usuarioService.GetByIdAsync(usuario.IdUsuario) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios2/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _usuarioService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
