using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorio.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class UsuarioService: IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepo;
        private readonly IMapper _mapper;
        public UsuarioService(IGenericRepository<Usuario> usuarioRepo, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioRepo = usuarioRepo;
        }
        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuarios = await _usuarioRepo.Consultar();
                var listaUsuario = queryUsuarios.Include(rol => rol.IdRolNavigation).ToList();
                
                return _mapper.Map<List<UsuarioDTO>>(listaUsuario);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuarios = await _usuarioRepo.Consultar(u => 
                u.Correo == correo &&
                u.Clave == clave);

                if(queryUsuarios.FirstOrDefault() == null)
                    throw new TaskCanceledException("El usuario no existe");

                Usuario devolverUsuario = queryUsuarios.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<SesionDTO>(devolverUsuario);
            }
            catch
            {
                throw;
            }
        }
        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioCreado = await _usuarioRepo.Crear(_mapper.Map<Usuario>(modelo));

                if(usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("No se puede crear usuario");
                
                var query = await _usuarioRepo.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepo.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if(usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                bool respuesta = await _usuarioRepo.Editar(usuarioEncontrado);

                if(!respuesta)
                    throw new TaskCanceledException("No se pudo editar usuario");

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
                var usuarioEncontrado = await _usuarioRepo.Obtener(u => u.IdUsuario == id);

                if(usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");
                
                bool respuesta = await _usuarioRepo.Eliminar(usuarioEncontrado);

                if(!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar usuario");

                return respuesta;
            }
            catch
            {
                throw;
            }
        }        
    }
}