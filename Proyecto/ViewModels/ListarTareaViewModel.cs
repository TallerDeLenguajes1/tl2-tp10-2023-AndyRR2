using System.ComponentModel.DataAnnotations;

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class ListarTareaViewModel{
        [Display(Name = "Id")]
        public int? Id{get;set;}

        [Display(Name = "Id Tablero")]
        public int? IdTablero{get;set;}

        [Display(Name = "Estado")]
        public string? Nombre{get;set;}

        [Display(Name = "Descripcion")]
        public EstadoTarea EstadoTarea{get;set;}

        [Display(Name = "Descripcion")]
        public string? Descripcion{get;set;}

        [Display(Name = "Color")]
        public Color Color{get;set;}

        [Display(Name = "Id Usuario Asignado")]
        public int? IdUsuarioAsignado{get;set;}

        [Display(Name = "Id Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}

        public ListarTareaViewModel(){}
        public ListarTareaViewModel(int? id, int? idTablero, string? nombre, EstadoTarea estado, string? descripcion, Color color, int? idUsuarioAsig, int? idUsuarioProp){
            Id=id;
            IdTablero=idTablero;
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            IdUsuarioAsignado=idUsuarioAsig;
            IdUsuarioPropietario=idUsuarioProp;
        }
        public static List<ListarTareaViewModel> FromTarea(List<Tarea> tareas)
        {
            List<ListarTareaViewModel> ListaTareaVM = new List<ListarTareaViewModel>();
            
                foreach (var tarea in tareas)
                {
                    ListarTareaViewModel newTareaVM = new ListarTareaViewModel();
                    newTareaVM.Id=tarea.Id;
                    newTareaVM.IdTablero=tarea.IdTablero;
                    newTareaVM.Nombre=tarea.Nombre;
                    newTareaVM.EstadoTarea=tarea.EstadoTarea;
                    newTareaVM.Descripcion=tarea.Descripcion;
                    newTareaVM.Color=tarea.Color;
                    newTareaVM.IdUsuarioAsignado=tarea.IdUsuarioAsignado;
                    newTareaVM.IdUsuarioPropietario=tarea.IdUsuarioPropietario;
                    ListaTareaVM.Add(newTareaVM);
                }
                return(ListaTareaVM);
        }
    }
}