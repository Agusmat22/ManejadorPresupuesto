using ManejadorPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManejadorPresupuesto.Models
{
    public class TipoCuenta 
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Debe completar el campo {0}")]
        [DisplayName("Nombre de Tipo de cuenta")]

        //agrego la accion para esta propiedad
        [Remote(action: "VerificarExisteTipoCuenta",controller:"TiposCuentas")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

       
    }
}
