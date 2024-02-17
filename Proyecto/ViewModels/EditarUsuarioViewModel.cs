using System.ComponentModel;
using System.ComponentModel.DataAnnotations;//Necesario para el uso de los Atributos de metadatos en las Propiedades del Modelo

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class EditarUsuarioViewModel{
        public int? Id{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(20)]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z]).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula.")]
        [MinLength(8)]
        [Display(Name = "Contraseña Actual")]
        public string? ContraseniaActual{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z]).+$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula.")]
        [MinLength(8)]
        [Display(Name = "Nueva Contraseña")]
        public string? Contrasenia{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nivel de Acceso")]
        public NivelDeAcceso NivelDeAcceso{get;set;}

        public EditarUsuarioViewModel(){}
        public EditarUsuarioViewModel(int? id, string? nombre, string? contrasenia, string? contraseniaActual, NivelDeAcceso nivel){
            Id=id;
            Nombre=nombre;
            Contrasenia=contrasenia;
            ContraseniaActual=contraseniaActual;
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