using ManejadorPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using ManejadorPresupuesto.Models.Interfaces;

namespace ManejadorPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {

        private readonly IRepositorioTipoCuentas repositorio;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTipoCuentas repositorio, IServicioUsuarios servicioUsuarios)
        {
            this.repositorio = repositorio;
            this.servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        public IActionResult Crear()
        {
            
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {

            int usuarioId = 1;

            IEnumerable<TipoCuenta> tipoCuentas = await this.repositorio.Obtener(usuarioId);

            return View(tipoCuentas);

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            int usuarioId = this.servicioUsuarios.ObtenerUsuarioId();

            TipoCuenta tipoCuenta = await this.repositorio.ObtenerPorId(id, usuarioId);

            if(tipoCuenta is null)
            {

                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipocuenta)
        {

            int usuarioId = this.servicioUsuarios.ObtenerUsuarioId();
            TipoCuenta cuentaEncontrada = await this.repositorio.ObtenerPorId(tipocuenta.Id, usuarioId);

            if(tipocuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await this.repositorio.Actualizar(tipocuenta);

            return RedirectToAction("Index");

        }


        [HttpPost]  //Retorno task ya que la consulta con sql es asincrona
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            //Esto valida si el objeto TipoCuenta es valido osea, no sea nullo
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            //aca estoy harcodeando el id
            tipoCuenta.UsuarioId = 1;

            //valido si existe el nombre de usuario y id en el tipo
            if(await this.repositorio.Existe(tipoCuenta.Nombre,tipoCuenta.UsuarioId))
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }


            await repositorio.Crear(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            int usuarioId = this.servicioUsuarios.ObtenerUsuarioId();
            TipoCuenta tipoCuenta = await this.repositorio.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuentaPorId(int id)
        {
            int usuarioId = this.servicioUsuarios.ObtenerUsuarioId();
            TipoCuenta tipoCuenta = await this.repositorio.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null) 
            { 
                return RedirectToAction("NoEncontrado","Home");
            }

            await this.repositorio.Borrar(id);

            return RedirectToAction("Index");
        }


        [HttpGet]
        /// <summary>
        /// Valida si el nombre de usuario ya existe, y muestra el mensaje en tiempo de escritura
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            //aca estoy harcodeando el id
            int usuarioId = 1;

            //valido si existe el nombre de usuario y id en el tipo
            if (await this.repositorio.Existe(nombre, usuarioId))
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }
    }
}
