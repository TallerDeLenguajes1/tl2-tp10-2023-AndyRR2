using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class EliminarUsuarioViewModel{
        public int? Id{get;set;}//No necesita atributos ya que va oculto en el View

        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña del Usuario a Eliminar")]
        public string? ContraseniaActual{get;set;}

        public EliminarUsuarioViewModel(){}
        public EliminarUsuarioViewModel(int? id, string? contraseniaActual){
            Id=id;
            ContraseniaActual=contraseniaActual;
        }
        public static EliminarUsuarioViewModel FromUsuario(Usuario usuario){
            return new EliminarUsuarioViewModel
            {
                Id=usuario.Id,
                ContraseniaActual=usuario.Contrasenia,
            };
        }
    }
}