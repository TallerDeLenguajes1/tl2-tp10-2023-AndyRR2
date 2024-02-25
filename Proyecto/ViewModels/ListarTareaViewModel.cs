using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class ListarTareaViewModel{
        public int? Id{get;set;}
        public string? Nombre{get;set;}
        public EstadoTarea EstadoTarea{get;set;}
        public string? Descripcion{get;set;}
        public Color Color{get;set;}
        public int? IdTablero{get;set;}
        public string? NombreTablero{get;set;}
        public int? IdUsuarioAsignado{get;set;}
        public string? NombreUsuarioAsignado{get;set;}
        public int? IdUsuarioPropietario{get;set;}
        public string? NombreUsuarioPropietario{get;set;}

        public ListarTareaViewModel(){}
        public ListarTareaViewModel(string? nombreTab, string? nombreProp, string? nombreAsign, int? id, int? idTablero, string? nombre, EstadoTarea estado, string? descripcion, Color color, int? idUsuarioAsig, int? idUsuarioProp){
            Id=id;
            IdTablero=idTablero;
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            IdUsuarioAsignado=idUsuarioAsig;
            IdUsuarioPropietario=idUsuarioProp;
            NombreUsuarioPropietario=nombreProp;
            NombreUsuarioAsignado=nombreAsign;
            NombreTablero=nombreTab;
        }
        public static List<ListarTareaViewModel> FromTarea(List<Tarea> tareas)
        {
            List<ListarTareaViewModel> ListaTareaVM = new List<ListarTareaViewModel>();
            
            foreach (var tarea in tareas)
            {
                ListarTareaViewModel newTareaVM = new ListarTareaViewModel();
                newTareaVM.Id=tarea.Id;
                newTareaVM.Nombre=tarea.Nombre;
                newTareaVM.EstadoTarea=tarea.EstadoTarea;
                newTareaVM.Descripcion=tarea.Descripcion;
                newTareaVM.Color=tarea.Color;
                newTareaVM.IdTablero=tarea.TableroPropio.Id;
                newTareaVM.NombreTablero=tarea.TableroPropio.Nombre;
                newTareaVM.IdUsuarioAsignado=tarea.Asignado.Id;
                newTareaVM.NombreUsuarioAsignado=tarea.Asignado.Nombre;
                newTareaVM.IdUsuarioPropietario=tarea.Propietario.Id;
                newTareaVM.NombreUsuarioPropietario=tarea.Propietario.Nombre;
                ListaTareaVM.Add(newTareaVM);
            }
            return(ListaTareaVM);
        }
    }
}