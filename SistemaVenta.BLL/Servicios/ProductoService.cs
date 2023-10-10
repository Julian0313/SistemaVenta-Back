using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepo;
        private readonly IMapper _mapper;
        public ProductoService(IGenericRepository<Producto> productoRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productoRepo = productoRepo;
        }        

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryProducto  = await _productoRepo.Consultar();
                var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();

                return _mapper.Map<List<ProductoDTO>>(listaProductos.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var productoCreado = await _productoRepo.Crear(_mapper.Map<Producto>(modelo));

                if(productoCreado.IdProducto == 0)
                    throw new TaskCanceledException("No se pudo crear producto");
                
                return _mapper.Map<ProductoDTO>(productoCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoEncontrado = await _productoRepo.Obtener(p=>
                    p.IdProducto == productoModelo.IdProducto);

                if(productoEncontrado.IdProducto == 0)
                    throw new TaskCanceledException("El producto no existe");

                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.EsActivo = productoModelo.EsActivo;

                bool respuesta = await _productoRepo.Editar(productoEncontrado);

                 if(!respuesta)
                    throw new TaskCanceledException("No se pudo editar producto");

                return respuesta;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncontrado = await _productoRepo.Obtener(p=>p.IdProducto == id);

                if(productoEncontrado == null)
                    throw new TaskCanceledException("El Producto no existe");

                bool respuesta = await _productoRepo.Eliminar(productoEncontrado);

                if(!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar producto");
                
                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}