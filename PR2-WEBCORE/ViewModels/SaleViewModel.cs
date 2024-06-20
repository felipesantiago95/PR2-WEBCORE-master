using PR2_WEBCORE.Models;
using System.Collections.Generic;

namespace PR2_WEBCORE.ViewModels
{
    public class SaleViewModel
    {
        public Sale Sale { get; set; }
        public IEnumerable<Usuario> Usuarios { get; set; }
        public IEnumerable<Establecimiento> Establecimientos { get; set; }
        public IEnumerable<Categorium> Categorias { get; set; }
        public IEnumerable<Aforo> Aforos { get; set; }
        public IEnumerable<Clase> Clases { get; set; }
    }
}
