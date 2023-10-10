using System.Globalization;
using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol,RolDTO>().ReverseMap();
            #endregion Rol

            #region Menu
            CreateMap<Menu,MenuDTO>().ReverseMap();
            #endregion Menu

            #region Categoria
            CreateMap<Categoria,CategoriaDTO>().ReverseMap();
            #endregion Categoria

            #region Usuario
            CreateMap<Usuario,UsuarioDTO>()
                .ForMember(dest => 
                    dest.RolDescripcion, 
                    opt => opt.MapFrom(org => org.IdRolNavigation.Nombre)
                )
                .ForMember(dest =>
                    dest.EsActivo,
                    opt => opt.MapFrom(org => org.EsActivo == true ? 1:0)
                );
            CreateMap<Usuario,SesionDTO>()
                .ForMember(dest => 
                    dest.RolDescripcion, 
                    opt => opt.MapFrom(org => org.IdRolNavigation.Nombre)
                );
             CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest =>
                    dest.IdRolNavigation,
                    opt  => opt.Ignore()
                )
                .ForMember(dest =>
                    dest.EsActivo,
                    opt => opt.MapFrom(org => org.EsActivo == 1 ? true : false)
                ); 
            #endregion Usuario

            #region Producto
            CreateMap<Producto,ProductoDTO>()
                .ForMember(dest =>
                    dest.DescripcionCategoria,
                    opt => opt.MapFrom(org => org.IdCategoriaNavigation.Nombre)
                )
                .ForMember(dest=>
                    dest.Precio,
                    opt=>opt.MapFrom(org=> Convert.ToString(org.Precio.Value, new CultureInfo("es-PE")))
                )
                .ForMember(dest =>
                    dest.EsActivo,
                    opt => opt.MapFrom(org => org.EsActivo == true ? 1:0)
                );
            CreateMap<ProductoDTO, Producto>()
                .ForMember(dest =>
                    dest.IdCategoriaNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(dest=>
                    dest.Precio,
                    opt=>opt.MapFrom(org=> Convert.ToDecimal(org.Precio, new CultureInfo("es-PE")))
                )
                .ForMember(dest =>
                    dest.EsActivo,
                    opt => opt.MapFrom(org => org.EsActivo == 1 ? true : false)
                );
            
            #endregion Producto

            #region Venta
            CreateMap<Venta,VentaDTO>()
                .ForMember(dest=>
                    dest.Total,
                    opt=>opt.MapFrom(org=> Convert.ToString(org.Total.Value, new CultureInfo("es-PE")))
                )
                .ForMember(dest=>
                    dest.FechaRegistro,
                    opt=>opt.MapFrom(org=> org.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                );
            CreateMap<VentaDTO, Venta>()
                .ForMember(dest=>
                    dest.Total,
                    opt=>opt.MapFrom(org=> Convert.ToDecimal(org.Total, new CultureInfo("es-PE")))
                );
            #endregion Venta

            #region DetalleVenta
            CreateMap<DetalleVenta,DetalleVentaDTO>()
                .ForMember(dest => 
                    dest.DescripcionProducto, 
                    opt => opt.MapFrom(org => org.IdProductoNavigation.Nombre)
                )
                .ForMember(dest=>
                    dest.Precio,
                    opt=>opt.MapFrom(org=> Convert.ToString(org.Precio.Value, new CultureInfo("es-PE")))
                )
                .ForMember(dest=>
                    dest.Total,
                    opt=>opt.MapFrom(org=> Convert.ToString(org.Total.Value, new CultureInfo("es-PE")))
                );
             CreateMap<DetalleVentaDTO, DetalleVenta>()
                .ForMember(dest =>
                    dest.IdProductoNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(dest=>
                    dest.Precio,
                    opt=>opt.MapFrom(org=> Convert.ToDecimal(org.Precio, new CultureInfo("es-PE")))
                )
                .ForMember(dest=>
                    dest.Total,
                    opt=>opt.MapFrom(org=> Convert.ToDecimal(org.Total, new CultureInfo("es-PE")))
                );
            #endregion DetalleVenta

            #region Reporte
            CreateMap<DetalleVenta,ReporteDTO>()
                .ForMember(dest=>
                    dest.FechaRegistro,
                    opt=>opt.MapFrom(org=> org.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                )
                .ForMember(dest=>
                    dest.NumeroDocummento,
                    opt=>opt.MapFrom(org=> org.IdVentaNavigation.NumeroDocumento)
                )
                .ForMember(dest=>
                    dest.TipoPago,
                    opt=>opt.MapFrom(org=> org.IdVentaNavigation.TipoPago)
                )
                .ForMember(dest=>
                    dest.TotalVenta,
                    opt=>opt.MapFrom(org=> Convert.ToString(org.IdVentaNavigation.Total.Value, new CultureInfo("es-PE")))
                )
                .ForMember(dest=>
                    dest.Producto,
                    opt=>opt.MapFrom(org=> org.IdProductoNavigation.Nombre)
                )
                .ForMember(dest=>
                    dest.Precio,
                    opt=>opt.MapFrom(org=> Convert.ToString(org.Precio.Value, new CultureInfo("es-PE")))
                )
                .ForMember(dest=>
                    dest.Total,
                    opt=>opt.MapFrom(org=> Convert.ToString(org.Total.Value, new CultureInfo("es-PE")))
                );
            #endregion Reporte
        }
    }
}