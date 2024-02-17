using Proyecto.ViewModels;

namespace Proyecto.Models{
    public enum EstadoTarea{
        ideas=1, 
        toDo=2, 
        doing=3, 
        review=4, 
        done=5,
        unnactive=6
    }
    public enum Color{
        azul=1,
        rojo=2,
        amarillo=3,
        verde=4,
        rosa=5,
        morado=6
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

        public static Tarea FromCrearTareaViewModel(CrearTareaViewModel tareaVM)
        {
            return new Tarea
            {
                IdTablero = tareaVM.IdTablero,
                Nombre = tareaVM.Nombre,
                Descripcion = tareaVM.Descripcion,
                Color = (Proyecto.Models.Color)tareaVM.Color,
                EstadoTarea = (Proyecto.Models.EstadoTarea)tareaVM.EstadoTarea,
                IdUsuarioAsignado = 0,
                IdUsuarioPropietario = tareaVM.IdUsuarioPropietario
            };
        }

        public static Tarea FromEditarTareaViewModel(EditarTareaViewModel tareaVM)
        {
            return new Tarea
            {
                Id = tareaVM.Id,
                IdTablero = tareaVM.IdTablero,
                Nombre = tareaVM.Nombre,
                Descripcion = tareaVM.Descripcion,
                Color = (Proyecto.Models.Color)tareaVM.Color,
                EstadoTarea = (Proyecto.Models.EstadoTarea)tareaVM.EstadoTarea
            };
        }
        public static Tarea FromAsignarTareaViewModel(AsignarTareaViewModel tareaVM)
        {
            return new Tarea
            {
                Id = tareaVM.Id,
                IdUsuarioAsignado = tareaVM.IdUsuarioAsignado
            };
        }
        
    }
}