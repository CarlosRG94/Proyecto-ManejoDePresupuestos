using ManejoDePresupuestos.Models;

namespace ManejoDePresupuestos.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Crear(Cuenta cuenta);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task<Cuenta> ObtenerPorId(int usuarioId, int id);
        Task Actualizar(CuentaCreacionViewModel cuenta);
        Task Borrar(int id);
    }
}
