﻿using System.ComponentModel.DataAnnotations;

namespace ManejoDePresupuestos.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:50,ErrorMessage ="No puede tener mas de {1} carácteres")]
        public string Nombre { get;set; }
        [Display (Name ="Tipo Operación")]
        public TipoOperacion TipoOperacionId { get; set; }
        public int UsuarioId { get; set; }  
    }
}
