using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepo;
        private readonly IGenericRepository<MenuRol> _menuRolRepo;
        private readonly IGenericRepository<Menu> _menuRepo;
        private readonly IMapper _mapper;
        public MenuService(IGenericRepository<Usuario> usuarioRepo,
            IGenericRepository<MenuRol> menuRolRepo, 
            IGenericRepository<Menu> menuRepo, 
            IMapper mapper)
        {
            _mapper = mapper;
            _menuRepo = menuRepo;
            _menuRolRepo = menuRolRepo;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario>tbUsuario = await _usuarioRepo.Consultar(u=>u.IdUsuario == idUsuario);
            IQueryable<MenuRol>tbMenuRol = await _menuRolRepo.Consultar();
            IQueryable<Menu>tbMenu = await _menuRepo.Consultar();
            try
            {
                IQueryable<Menu>tbResultado = (from u in tbUsuario
                                join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                join m in tbMenu on mr.IdMenu equals m.IdMenu
                                select m).AsQueryable();

                var listaMenus = tbResultado.ToList();
                return _mapper.Map<List<MenuDTO>>(listaMenus);
            }
            catch
            {
                throw;
            }
        }
    }
}