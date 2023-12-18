namespace ManejadorPresupuesto.Models.Interfaces
{
    public interface IRepositorioTipoCuentas
    {
        public Task Crear(TipoCuenta tipoCuenta);
        public Task<bool> Existe(string nombre, int usuarioId);

        public Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);

        public Task Actualizar(TipoCuenta tipoCuenta);

        public Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);

        public Task Borrar(int id);



    }
}
