using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class CrearUsuarioViewModel{

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(20)]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z]).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula.")]
        [MinLength(8)]
        [Display(Name = "Contraseña")]
        public string? Contrasenia{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nivel de Acceso")]
        public NivelDeAcceso NivelDeAcceso{get;set;}

        public CrearUsuarioViewModel(){}
        public CrearUsuarioViewModel(string? nombre, string? contrasenia, NivelDeAcceso nivel){
            Nombre=nombre;
            Contrasenia=contrasenia;
            NivelDeAcceso=nivel;
        }
    }
}