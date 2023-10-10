using System.Globalization;
using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IGenericRepository<Producto> _productoRepo;
        private readonly IVentaRepository _ventaRepo;
        private readonly IMapper _mapper;
        public DashBoardService(IGenericRepository<Producto> productoRepo, 
            IVentaRepository ventaRepo, 
            IMapper mapper)
        {
            _mapper = mapper;
            _ventaRepo = ventaRepo;
            _productoRepo = productoRepo;
        }
        private IQueryable<Venta> RetornarVenta(IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            DateTime?ultimaFecha = tablaVenta.OrderByDescending(v=>v.FechaRegistro).Select(v=>v.FechaRegistro).First();

            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v=>v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }
        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepo.Consultar();

            if(_ventaQuery.Count()>0)
            {
                var tablaVenta = RetornarVenta(_ventaQuery,-7);
                total = tablaVenta.Count();
            }
            return total;
        }
        private async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            IQueryable<Venta>_ventaQuery = await _ventaRepo.Consultar();

            if(_ventaQuery.Count()>0)
            {
                var tablaVenta = RetornarVenta(_ventaQuery,-7);
                resultado = tablaVenta.Select(v=>v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-PE"));
        }
        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> _productoQuery = await _productoRepo.Consultar();

            int total = _productoQuery.Count();
            return total;
        }
        private async Task<Dictionary<string,int>>VentaUltimaSemana()
        {
            Dictionary<string,int>resultado = new Dictionary<string, int>();
            IQueryable<Venta> _ventaQuery = await _ventaRepo.Consultar();

            if(_ventaQuery.Count()>0)
            {
                var tablaVenta = RetornarVenta(_ventaQuery,-7);

                resultado = tablaVenta
                    .GroupBy(v=>v.FechaRegistro.Value.Date).OrderBy(g=>g.Key)
                    .Select(dv=>new{fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count()})
                    .ToDictionary(keySelector: r=>r.fecha,elementSelector:r=>r.total);
            } 
            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashBoard = new DashBoardDTO();
            try
            {
                vmDashBoard.TotalVentas = await TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await TotalProductos();

                List<VentaSemanaDTO> listaVentaSemana = new List<VentaSemanaDTO>();

                foreach(KeyValuePair<string,int>item in await VentaUltimaSemana())
                {
                    listaVentaSemana.Add(new VentaSemanaDTO(){
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                vmDashBoard.VentasUltimaSemana = listaVentaSemana;
            }            
            catch
            {
                throw;
            }
            return vmDashBoard;
        }
    }
}