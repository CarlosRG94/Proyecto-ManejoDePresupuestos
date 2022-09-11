using ManejoDePresupuestos.Models;

namespace ManejoDePresupuestos.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task Editar(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
}
