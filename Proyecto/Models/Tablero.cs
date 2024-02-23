using Proyecto.ViewModels;

namespace Proyecto.Models{
    public enum EstadoTablero{
        Active=1,
        Unnactive=2
    }
    public class Tablero{
        public int? Id{get;set;}
        public Usuario Propietario{get;set;}
        public string? Nombre{get;set;}
        public string? Descripcion{get;set;}
        public EstadoTablero EstadoTablero{get;set;}

        public Tablero(){
            
        }
        
        public Tablero(int? id, int? idUsu, string? nombreUsu, string? nombre, string? descripcion, EstadoTablero estado){
            Id=id;
            Propietario = new Usuario(idUsu, nombreUsu);
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
        }
        public static Tablero FromCrearTableroViewModel(CrearTableroViewModel tableroVM)
        {
            return new Tablero
            {
                Propietario = new Usuario(tableroVM.IdUsuarioPropietario,tableroVM.Nombre),
                Nombre = tableroVM.Nombre,
                Descripcion=tableroVM.Descripcion,
                EstadoTablero = (Proyecto.Models.EstadoTablero)tableroVM.EstadoTablero
            };
        }
        public static Tablero FromEditarTableroViewModel(EditarTableroViewModel tableroVM)//Solo se crea con las propiedades editables
        {
            return new Tablero
            {
                Id = tableroVM.Id,
                Propietario = new Usuario(tableroVM.IdUsuarioPropietario,tableroVM.Nombre),
                Nombre = tableroVM.Nombre,
                Descripcion=tableroVM.Descripcion,
                EstadoTablero = (Proyecto.Models.EstadoTablero)tableroVM.EstadoTablero
            };
        }
    }
}