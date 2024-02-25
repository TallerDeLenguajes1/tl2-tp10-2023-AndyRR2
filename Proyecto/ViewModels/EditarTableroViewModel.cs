using System.ComponentModel.DataAnnotations;//Necesario para uso de Atributos

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class EditarTableroViewModel{
        public int? Id{get;set;}//No necesita atributos ya que va oculto en el View

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Usuario Propietario")]
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

        public List<Usuario> Usuarios{get;set;}//Necesario para guardar la lista de Usuarios seleccionables obtenidos de la DB
        public EditarTableroViewModel(){
            Usuarios = new List<Usuario>();//Asegura que siempre tenga una instancia de la lista válida
        }
        public EditarTableroViewModel(int? id, int? idUsu, string? nombre, string? descripcion, EstadoTablero estado, List<Usuario> listaUsu){
            Id=id;
            IdUsuarioPropietario=idUsu;
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
            Usuarios=listaUsu;
        }
        public static EditarTableroViewModel FromTablero(Tablero tablero)
        {
            return new EditarTableroViewModel
            {
                Id = tablero.Id,
                IdUsuarioPropietario = tablero.Propietario.Id,
                Nombre = tablero.Nombre,
                Descripcion=tablero.Descripcion,
                EstadoTablero = (Proyecto.Models.EstadoTablero)tablero.EstadoTablero
            };
        }
    }
}