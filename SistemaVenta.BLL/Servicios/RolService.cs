using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _rolRepo;
        private readonly IMapper _mapper;
        public RolService(IGenericRepository<Rol> rolRepo, IMapper mapper)
        {
            _mapper = mapper;
            _rolRepo = rolRepo;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try
            {
                var listaRoles = await _rolRepo.Consultar();
                return _mapper.Map<List<RolDTO>>(listaRoles.ToList());
            }
            catch
            {
                throw;
            }
        }
    }
}