using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class EditarTableroViewModel{
        [Display(Name = "Id")]
        public int? Id{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id de Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre Tablero")]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Descripción")]
        public string? Descripcion{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")]
        public EstadoTablero EstadoTablero{get;set;}

        public List<int?> IdUsuarios{get;set;}

        public EditarTableroViewModel(){
            IdUsuarios = new List<int?>();
        }
        public EditarTableroViewModel(int? id, int? idUsu, string? nombre, string? descripcion, EstadoTablero estado, List<int?> idUsuarios){
            Id=id;
            IdUsuarioPropietario=idUsu;
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
            IdUsuarios=idUsuarios;
        }
        public static EditarTableroViewModel FromTablero(Tablero tablero)
        {
            return new EditarTableroViewModel
            {
                Id = tablero.Id,
                IdUsuarioPropietario = tablero.IdUsuarioPropietario,
                Nombre = tablero.Nombre,
                Descripcion=tablero.Descripcion,
                EstadoTablero = (Proyecto.Models.EstadoTablero)tablero.EstadoTablero
            };
        }
        
    }
}