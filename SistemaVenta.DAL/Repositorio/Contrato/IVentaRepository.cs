using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorio.Contrato
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta modelo);
    }
}