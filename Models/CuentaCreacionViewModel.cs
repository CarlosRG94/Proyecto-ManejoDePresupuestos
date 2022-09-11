using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoDePresupuestos.Models
{
    public class CuentaCreacionViewModel:Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
