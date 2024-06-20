﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2_WEBCORE.Models
{
    public partial class Sale
    {
        public int IdSales { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstablecimiento { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdAforo { get; set; }
        public int? IdClase { get; set; }
        public decimal? Total { get; set; }
        public DateTime FechaCreacion { get; set; } // Nuevo campo
        public virtual Aforo IdAforoNavigation { get; set; }
        public virtual Categorium IdCategoriaNavigation { get; set; }
        public virtual Clase IdClaseNavigation { get; set; }
        public virtual Establecimiento IdEstablecimientoNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
