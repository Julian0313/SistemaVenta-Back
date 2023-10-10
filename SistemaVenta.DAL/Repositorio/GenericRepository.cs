using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorio.Contrato;

namespace SistemaVenta.DAL.Repositorio
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbventaContext _dbContext;
        public GenericRepository(DbventaContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> Obtener(Expression<Func<T, bool>> filtro)
        {
            try
            {
                T modelo = await _dbContext.Set<T>().FirstOrDefaultAsync(filtro);
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> Crear(T modelo)
        {
            try
            {
                _dbContext.Set<T>().Add(modelo);
                await _dbContext.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(T modelo)
        {
            try
            {
                _dbContext.Set<T>().Update(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(T modelo)
        {
            try
            {
                _dbContext.Set<T>().Remove(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<T>>Consultar(Expression<Func<T, bool>> filtro = null)
        {
            try
            {
                IQueryable<T> queryModelo = filtro == null ? _dbContext.Set<T>(): _dbContext.Set<T>().Where(filtro);
                return queryModelo;
            }
            catch
            {
                throw;
            }
        }
    }
}