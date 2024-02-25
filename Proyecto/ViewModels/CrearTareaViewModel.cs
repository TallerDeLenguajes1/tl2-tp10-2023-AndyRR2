using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class CrearTareaViewModel{
        
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Tablero Propio")]
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
        [Display(Name = "Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}

        public List<Tablero> Tableros{get;set;}//Necesario para guardar la lista a seleccionables obtenidos de la DB
        public List<Usuario> Usuarios{get;set;}//Necesario para guardar la lista a seleccionables obtenidos de la DB
        public CrearTareaViewModel(){
            Tableros = new List<Tablero>();//Asegura que siempre tenga una instancia de la lista válida
            Usuarios = new List<Usuario>();//Asegura que siempre tenga una instancia de la lista válida
        }
        public CrearTareaViewModel(int? idTablero, string? nombre, EstadoTarea estado, string? descripcion, Color color, int? idUsuarioProp, List<Tablero> tableros, List<Usuario> usuarios){
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            IdTablero=idTablero;
            IdUsuarioPropietario=idUsuarioProp;
            Tableros=tableros;
            Usuarios=usuarios;
        }
        
    }
}