using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class CrearTableroViewModel{

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id de Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre Tablero")]
        [MaxLength(20)]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Descripción")]
        [MaxLength(30)]
        public string? Descripcion{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")]
        public EstadoTablero EstadoTablero{get;set;}

        public List<int?> IdUsuarios{get;set;}

        public CrearTableroViewModel(){
            IdUsuarios = new List<int?>();
        }
        public CrearTableroViewModel(int? idUsu, string? nombre, string? descripcion, EstadoTablero estado, List<int?> idUsuarios){
            IdUsuarioPropietario=idUsu;
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
            IdUsuarios=idUsuarios;
        }
        
    }
}