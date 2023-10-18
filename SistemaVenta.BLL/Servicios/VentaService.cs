using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepo;
        private readonly IVentaRepository _ventaRepo;
        public VentaService(IVentaRepository ventaRepo,
            IGenericRepository<DetalleVenta> detalleVentaRepo,
            IMapper mapper)
        {
            _ventaRepo = ventaRepo;
            _detalleVentaRepo = detalleVentaRepo;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaGenerada = await _ventaRepo.Registrar(_mapper.Map<Venta>(modelo));

                if(ventaGenerada.IdVenta == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _mapper.Map<VentaDTO>(ventaGenerada);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {             
            IQueryable<Venta> query = await _ventaRepo.Consultar();//Generar consulta para tabla de venta para posteriormente contruirla
            var listaResultado = new List<Venta>();
            try
            {
                if(buscarPor == "fecha")
                {
                    DateTime fech_Inicio = DateTime.ParseExact(fechaInicio,"dd/MM/yyyy", new CultureInfo("es-PER"));
                    DateTime fech_Fin = DateTime.ParseExact(fechaFin,"dd/MM/yyyy", new CultureInfo("es-PER"));

                    listaResultado = await query.Where(v=>
                        v.FechaRegistro.Value.Date >= fech_Inicio.Date &&
                        v.FechaRegistro.Value.Date <= fech_Fin.Date)
                            .Include(dv => dv.DetalleVenta)
                            .ThenInclude(p=>p.IdProductoNavigation)
                            .ToListAsync(); 
                }
                else{
                    listaResultado = await query.Where(v=>v.NumeroDocumento == numeroVenta)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p =>p.IdProductoNavigation)
                        .ToListAsync();
                }
            }
            catch
            {
                throw;
            }
            return _mapper.Map<List<VentaDTO>>(listaResultado);
        }       

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepo.Consultar();
            var listaResultado = new List<DetalleVenta>();
            try
            {
                DateTime fech_Inicio = DateTime.ParseExact(fechaInicio,"dd/MM/yyyy", new CultureInfo("es-PER"));
                DateTime fech_Fin = DateTime.ParseExact(fechaFin,"dd/MM/yyyy", new CultureInfo("es-PER"));

                listaResultado = await query
                    .Include(p=> p.IdProductoNavigation)
                    .Include(v=> v.IdVentaNavigation)
                    .Where(dv =>
                        dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_Inicio.Date &&
                        dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_Fin.Date).ToListAsync();
            }
            catch
            {
                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(listaResultado);
        }
    }
}