using Dapper;
using ManejadorPresupuesto.Models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ManejadorPresupuesto.Models.Servicios
{
    public class RepositorioTipoCuentas : IRepositorioTipoCuentas
    {
        private readonly string connectionString;

        public RepositorioTipoCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        public async Task Crear(TipoCuenta cuenta)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    var id = await sqlConnection.QuerySingleAsync<int>
                        ($@"INSERT INTO TiposCuentas (Nombre,UsuarioId,Orden) Values (@Nombre,@UsuarioId,0);SELECT SCOPE_IDENTITY();", cuenta);

                    cuenta.Id = id;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                //EN CASO QUE LO ENCUENTRE RETORNARA 1 Y EN CASO QUE NO RETORNARA 
                var resultado = await sqlConnection.QueryFirstOrDefaultAsync<int>(
                                        "SELECT 1 FROM TiposCuentas WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                                        new { Nombre = nombre, UsuarioId = usuarioId });

                return resultado == 1;
            }


        }

        /// <summary>
        /// Obtiene un lista de todos los servicios de cuenta mediante el usuarioId
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id,Nombre,Orden FROM TiposCuentas WHERE UsuarioId = @UsuarioId";

                IEnumerable<TipoCuenta> listado = await sqlConnection.QueryAsync<TipoCuenta>(query, new { UsuarioId = usuarioId });

                return listado;
            }
        }

        /// <summary>
        /// Obtiene un tipo cuenta mediante su Id y usuarioId, los dos deben coincidir
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {

            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                TipoCuenta tipoCuenta = await sqlConnection.QueryFirstOrDefaultAsync<TipoCuenta>
                                                                                (@"SELECT Id,Nombre,Orden 
                                                                                FROM TiposCuentas 
                                                                                WHERE Id=@Id AND UsuarioId=@UsuarioId", 
                                                                                new { Id = id, UsuarioId = usuarioId });

                return tipoCuenta;
            }



        }


        /// <summary>
        /// Actualiza un tipo cuenta de la db, si el ID y UsuarioId coinciden
        /// </summary>
        /// <param name="tipoCuenta"></param>
        /// <returns></returns>
        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {

                await sqlConnection.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre=@Nombre WHERE Id=@Id", tipoCuenta);

            }
        }

        public async Task Borrar(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE TiposCuentas WHERE Id=@Id", new { Id = id });
            }
        }



    }
}
