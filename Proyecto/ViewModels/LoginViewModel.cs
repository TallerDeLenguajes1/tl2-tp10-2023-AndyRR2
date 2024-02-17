using System.ComponentModel;
using System.ComponentModel.DataAnnotations;//Necesario para el uso de los Atributos de metadatos en las Propiedades del Modelo

namespace Proyecto.ViewModels{
    public class LoginViewModel{

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(20)]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Contrasenia{get;set;}

        public LoginViewModel(){}
        public LoginViewModel(string? nombre, string? contrasenia){
            Nombre=nombre;
            Contrasenia=contrasenia;
        }
        
    }
}