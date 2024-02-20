using Proyecto.ViewModels;

namespace Proyecto.Models{
    public enum EstadoTarea{
        Ideas=1, 
        ToDo=2, 
        Doing=3, 
        Review=4, 
        Done=5,
        Unnactive=6
    }
    public enum Color{
        Azul=1,
        Rojo=2,
        Amarillo=3,
        Verde=4,
        Rosa=5,
        Morado=6
    }
    public class Tarea{
        public int? Id{get;set;}
        public int? IdTablero{get;set;}
        public string? Nombre{get;set;}
        public EstadoTarea EstadoTarea{get;set;}
        public string? Descripcion{get;set;}
        public Color Color{get;set;}
        public int? IdUsuarioAsignado{get;set;}
        public int? IdUsuarioPropietario{get;set;}
        public Tarea(){}
        public Tarea(int? id, int? idTablero, string? nombre, EstadoTarea estado, string? descripcion, Color color, int? idUsuarioAsig, int? idUsuarioProp){
            Id=id;
            IdTablero=idTablero;
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            IdUsuarioAsignado=idUsuarioAsig;
            IdUsuarioPropietario=idUsuarioProp;
        }
        public static Tarea FromCrearTareaViewModel(CrearTareaViewModel tareaVM)//Usuario asignado es 0, luego se asigna en AsignarUsuario
        {
            return new Tarea
            {
                IdTablero = tareaVM.IdTablero,
                Nombre = tareaVM.Nombre,
                Descripcion = tareaVM.Descripcion,
                Color = (Proyecto.Models.Color)tareaVM.Color,
                EstadoTarea = (Proyecto.Models.EstadoTarea)tareaVM.EstadoTarea,
                IdUsuarioAsignado = null,//se asigna despues de que se crea
                IdUsuarioPropietario = tareaVM.IdUsuarioPropietario
            };
        }
        public static Tarea FromEditarTareaViewModel(EditarTareaViewModel tareaVM)//Solo se crea con las propiedades editables
        {
            return new Tarea
            {
                Id = tareaVM.Id,
                IdTablero = tareaVM.IdTablero,
                Nombre = tareaVM.Nombre,
                Descripcion = tareaVM.Descripcion,
                Color = (Proyecto.Models.Color)tareaVM.Color,
                EstadoTarea = (Proyecto.Models.EstadoTarea)tareaVM.EstadoTarea,
                IdUsuarioPropietario = tareaVM.IdUsuarioPropietario
            };
        }
        public static Tarea FromAsignarTareaViewModel(AsignarTareaViewModel tareaVM)//Solo se crea con las propiedades necesarias
        {
            return new Tarea
            {
                Id = tareaVM.Id,
                IdUsuarioAsignado = tareaVM.IdUsuarioAsignado
            };
        }

        public static Tarea FromCambiarEstadoTareaViewModel(CambiarEstadoTareaViewModel tareaVM)//Solo se crea con las propiedades necesarias
        {
            return new Tarea
            {
                Id = tareaVM.Id,
                EstadoTarea = tareaVM.EstadoTarea
            };
        }
    }
}