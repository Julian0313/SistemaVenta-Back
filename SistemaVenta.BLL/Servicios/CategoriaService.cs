using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoriaRepo;
        private readonly IMapper _mapper;
        public CategoriaService(IGenericRepository<Categoria> categoriaRepo, IMapper mapper)
        {
            _mapper = mapper;
            _categoriaRepo = categoriaRepo;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var listaCategoria = await _categoriaRepo.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategoria.ToList());
            }
            catch
            {
                throw;
            }
        }
    }
}