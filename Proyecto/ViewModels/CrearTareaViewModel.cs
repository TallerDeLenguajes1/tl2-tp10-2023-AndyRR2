using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class CrearTareaViewModel{
        
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Tablero")]
        public int? IdTablero{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]
        [MaxLength(20)]
        public string? Nombre{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")]
        public EstadoTarea EstadoTarea{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Descripcion")]
        [MaxLength(30)]
        public string? Descripcion{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Color")]
        public Color Color{get;set;}

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Id Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}

        public List<int?> IdTableros{get;set;}//Necesario para guardar la lista de Id seleccionables obtenidos de la DB
        public List<int?> IdUsuarios{get;set;}//Necesario para guardar la lista de Id seleccionables obtenidos de la DB
        public CrearTareaViewModel(){
            IdTableros = new List<int?>();//Asegura que siempre tenga una instancia de la lista válida
            IdUsuarios = new List<int?>();//Asegura que siempre tenga una instancia de la lista válida
        }
        public CrearTareaViewModel(int? idTablero, string? nombre, EstadoTarea estado, string? descripcion, Color color, int? idUsuarioProp, List<int?> idTableros, List<int?> idUsuarios){
            IdTablero=idTablero;
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            IdUsuarioPropietario=idUsuarioProp;
            IdTableros=idTableros;
            IdUsuarios=idUsuarios;
        }
        
    }
}