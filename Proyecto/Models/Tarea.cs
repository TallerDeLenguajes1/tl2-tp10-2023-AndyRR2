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
        public string? Nombre{get;set;}
        public EstadoTarea EstadoTarea{get;set;}
        public string? Descripcion{get;set;}
        public Color Color{get;set;}
        public Usuario Propietario{get;set;}
        public Usuario Asignado{get;set;}
        public Tablero TableroPropio{get;set;}
        public Tarea(){
            Propietario = new Usuario();
            Asignado = new Usuario();
            TableroPropio = new Tablero();
        }
        public Tarea(int? id, int? idTablero, string? nombreProp, string? nombreAsig, string nombreTab,string? nombre, EstadoTarea estado, string? descripcion, Color color, int? idUsuarioAsig, int? idUsuarioProp){
            Id=id;
            Nombre=nombre;
            EstadoTarea=estado;
            Descripcion=descripcion;
            Color=color;
            Propietario = new Usuario(idUsuarioProp, nombreProp);
            Asignado = new Usuario(idUsuarioAsig, nombreAsig);
            TableroPropio = new Tablero(idTablero, nombreTab);
        }
        public static Tarea FromCrearTareaViewModel(CrearTareaViewModel tareaVM)//Usuario asignado es 0, luego se asigna en AsignarUsuario
        {
            return new Tarea
            {
                Propietario = new Usuario(tareaVM.IdUsuarioPropietario,null),
                Asignado = new Usuario(null,null),
                TableroPropio = new Tablero(tareaVM.IdTablero,null),
                Nombre = tareaVM.Nombre,
                Descripcion = tareaVM.Descripcion,
                Color = (Proyecto.Models.Color)tareaVM.Color,
                EstadoTarea = (Proyecto.Models.EstadoTarea)tareaVM.EstadoTarea,
            };
        }
        public static Tarea FromEditarTareaViewModel(EditarTareaViewModel tareaVM)//Solo se crea con las propiedades editables
        {
            return new Tarea
            {
                Propietario = new Usuario(tareaVM.IdUsuarioPropietario,null),
                Asignado = new Usuario(null,null),
                TableroPropio = new Tablero(tareaVM.IdTablero,null),
                Id = tareaVM.Id,
                Nombre = tareaVM.Nombre,
                Descripcion = tareaVM.Descripcion,
                Color = (Proyecto.Models.Color)tareaVM.Color,
                EstadoTarea = (Proyecto.Models.EstadoTarea)tareaVM.EstadoTarea,
                
            };
        }
        public static Tarea FromAsignarTareaViewModel(AsignarTareaViewModel tareaVM)//Solo se crea con las propiedades necesarias
        {
            return new Tarea
            {
                Id = tareaVM.Id,
                Asignado = new Usuario(tareaVM.IdUsuarioAsignado,null)
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