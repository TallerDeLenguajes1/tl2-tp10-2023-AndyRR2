using Proyecto.ViewModels;

namespace Proyecto.Models{
    public enum EstadoTablero{
        Active=1,
        Unnactive=2
    }
    public class Tablero{
        public int? Id{get;set;}
        public int? IdUsuarioPropietario{get;set;}
        public string? Nombre{get;set;}
        public string? Descripcion{get;set;}
        public EstadoTablero EstadoTablero{get;set;}
        public Tablero(){}
        public Tablero(int? id, int? idUsu, string? nombre, string? descripcion, EstadoTablero estado){
            Id=id;
            IdUsuarioPropietario=idUsu;
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
        }
        public static Tablero FromCrearTableroViewModel(CrearTableroViewModel tableroVM)
        {
            return new Tablero
            {
                IdUsuarioPropietario = tableroVM.IdUsuarioPropietario,
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
                IdUsuarioPropietario = tableroVM.IdUsuarioPropietario,
                Nombre = tableroVM.Nombre,
                Descripcion=tableroVM.Descripcion,
                EstadoTablero = (Proyecto.Models.EstadoTablero)tableroVM.EstadoTablero
            };
        }
    }
}