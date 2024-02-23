using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class EditarTableroViewModel{
        public int? Id{get;set;}//No necesita atributos ya que va oculto en el View

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id de Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre Tablero")]
        [MaxLength(20)]
        public string? NombreUsuarioPropietario{get;set;}

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

        public List<int?> IdUsuarios{get;set;}//Necesario para guardar la lista de Id seleccionables obtenidos de la DB
        public EditarTableroViewModel(){
            IdUsuarios = new List<int?>();//Asegura que siempre tenga una instancia de la lista válida
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
                IdUsuarioPropietario = tablero.Propietario.Id,
                NombreUsuarioPropietario = tablero.Propietario.Nombre,
                Nombre = tablero.Nombre,
                Descripcion=tablero.Descripcion,
                EstadoTablero = (Proyecto.Models.EstadoTablero)tablero.EstadoTablero
            };
        }
    }
}