using System.ComponentModel.DataAnnotations;

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class CrearTareaViewModel{
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Tablero")]
        public int? IdTablero{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")]
        public EstadoTarea EstadoTarea{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Descripcion")]
        public string? Descripcion{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Color")]
        public Color Color{get;set;}

        [Display(Name = "Id Usuario Asignado")]
        public int? IdUsuarioAsignado{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}
        public List<int?> IdTableros{get;set;}
        public List<int?> IdUsuarios{get;set;}


        public CrearTareaViewModel(){
            IdTableros = new List<int?>();
            IdUsuarios = new List<int?>();
        }
        public CrearTareaViewModel(int? idTablero, string? nombre, EstadoTarea estado, string? descripcion, Color color, int? idUsuarioAsig, int? idUsuarioProp, List<int?> idTableros, List<int?> idUsuarios){
            IdTablero=idTablero;
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            IdUsuarioAsignado=idUsuarioAsig;
            IdUsuarioPropietario=idUsuarioProp;
            IdTableros=idTableros;
            IdUsuarios=idUsuarios;
        }
        
    }
}