using System.ComponentModel;
using System.ComponentModel.DataAnnotations;//Necesario para el uso de los Atributos de metadatos en las Propiedades del Modelo

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class EditarUsuarioViewModel{

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id")]
        public int? Id{get;set;}

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

        public EditarUsuarioViewModel(){}
        public EditarUsuarioViewModel(int? id, string? nombre, string? contrasenia, NivelDeAcceso nivel){
            Id=id;
            Nombre=nombre;
            Contrasenia=contrasenia;
            NivelDeAcceso=nivel;
        }
        public static EditarUsuarioViewModel FromUsuario(Usuario usuario){
            return new EditarUsuarioViewModel
            {
                Id=usuario.Id,
                Nombre=usuario.Nombre,
                Contrasenia=usuario.Contrasenia,
                NivelDeAcceso=usuario.NivelDeAcceso
            };
        }
        
    }
}