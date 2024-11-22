using Dapper;
using ManejoDePresupuestos.Models;
using ManejoDePresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoDePresupuestos.Controllers
{
    public class TiposCuentasController:Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios serviciousuarios;
        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
                               IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.serviciousuarios= servicioUsuarios;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = serviciousuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);
        }
        public IActionResult Crear()
        {
           
                return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid) 
            {
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId = serviciousuarios.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre,
                                                                      tipoCuenta.UsuarioId);
            if(yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),
                    $"El nombre  {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }
            await repositorioTiposCuentas.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usarioId = serviciousuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usarioId);
            if(tipoCuenta == null)
            {
                return RedirectToAction("NoEncontrado", "Home"); 
            }
            return View(tipoCuenta);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = serviciousuarios.ObtenerUsuarioId();
            var existeTipoCuenta = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id,usuarioId);
            if(tipoCuenta == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }
        public async Task <IActionResult> Borrar (int id)
        {
            var usuarioId = serviciousuarios.ObtenerUsuarioId();
            var tipocuenta = await repositorioTiposCuentas.ObtenerPorId(id,usuarioId);
            if(tipocuenta == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipocuenta);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = serviciousuarios.ObtenerUsuarioId();
            var tipocuentaexiste = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);
            if(tipocuentaexiste == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Borrar(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCUenta(string nombre, int id)
        {
            var usuarioId = serviciousuarios.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId, id);
            if (yaExisteTipoCuenta)
            {
                return Json($"El nombre{nombre} ya existe");
            }
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids) 
        {
            var usuarioId = serviciousuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select(x=>x.Id);

            var idsTiposCuentasNoPertenecenAlUsuario = ids.Except(idsTiposCuentas).ToList();

            if (idsTiposCuentasNoPertenecenAlUsuario.Count() > 0)
            {
                return Forbid();
            }
            var tiposCuentasOrdenados = ids.Select((valor, indice) =>
            new TipoCuenta() { Id = valor, Orden = indice + 1 }).AsEnumerable();
            await repositorioTiposCuentas.Ordenar(tiposCuentasOrdenados);
            return Ok();
        }

    }
}
