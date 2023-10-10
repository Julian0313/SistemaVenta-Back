using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorio;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.Utility;

namespace SistemaVenta.IOC
{
    public static class Dependencias
    {
        public static void inyectarDependencias(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DbventaContext>(opt =>{
                opt.UseSqlServer(config.GetConnectionString("ConnectionDefaault"));
            });
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<IMenuService,MenuService>();
            services.AddScoped<IProductoService,ProductoService>();
            services.AddScoped<IRolService,RolService>();
            services.AddScoped<IUsuarioService,UsuarioService>();
            services.AddScoped<IVentaService,VentaService>();
        }
    }
}