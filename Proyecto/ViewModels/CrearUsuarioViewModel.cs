using System.ComponentModel;
using System.ComponentModel.DataAnnotations;//Necesario para el uso de los Atributos de metadatos en las Propiedades del Modelo

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class CrearUsuarioViewModel{

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [PasswordPropertyText]
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