using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task<bool> ComprobarNombreExistente(TipoCuenta tipoCuenta);
        Task Crear(TipoCuenta tipoCuenta);
        Task<IEnumerable<TipoCuenta>> Obtener(TipoCuenta tipoCuenta);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioTiposCuentas: IRepositorioTiposCuentas
    {

        private readonly string connectionString;

        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                                    (@"INSERT INTO  TiposCuentas (Nombre, UsuarioId, Orden) 
                                        Values (@Nombre, @UsuarioId, 0);
                                        SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task<bool> ComprobarNombreExistente(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>
                                     (@"SELECT COUNT(*) 
                                         FROM TiposCuentas 
                                         WHERE UsuarioId = @UsuarioId AND Nombre = @Nombre;", tipoCuenta);
            if (existe == 0)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(TipoCuenta tipoCuenta)
        {
            tipoCuenta.UsuarioId = 1;
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden 
                                                             FROM TiposCuentas 
                                                             WHERE UsuarioId = @UsuarioId", tipoCuenta);
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas 
                                            SET Nombre = @Nombre 
                                            WHERE Id = @Id", tipoCuenta); 
        } 

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM TiposCuentas WHERE Id = @Id;", new { id });
        }

       
        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden 
                                                                           FROM TiposCuentas 
                                                                           WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId});
        }

    }
}
