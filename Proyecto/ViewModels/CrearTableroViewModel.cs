using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

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

        public List<Usuario> Usuarios{get;set;}//Necesario para guardar la lista de Id seleccionables obtenidos de la DB
        public CrearTableroViewModel(){
            Usuarios = new List<Usuario>();//Asegura que siempre tenga una instancia de la lista válida
        }
        public CrearTableroViewModel(int? idUsu, string? nombre, string? nombreUsu, string? descripcion, EstadoTablero estado, List<Usuario> listaUsu){
            IdUsuarioPropietario=idUsu;
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
            Usuarios=listaUsu;
        }
    }
}