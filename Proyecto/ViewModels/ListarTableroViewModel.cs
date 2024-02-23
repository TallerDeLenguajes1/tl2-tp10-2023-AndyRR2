using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class ListarTableroViewModel{
        public int? Id{get;set;}
        public string? Nombre{get;set;}
        public string? Descripcion{get;set;}
        public EstadoTablero EstadoTablero{get;set;}
        public int? IdUsuarioPropietario{get;set;}
        public string? NombreUsuarioPropietario{get;set;}
        public ListarTableroViewModel(){}
        public ListarTableroViewModel(int? id, string? nombre, string? descripcion, EstadoTablero estado, int? idUsu, string? nombreUsu){
            Id=id;
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
            IdUsuarioPropietario=idUsu;
            NombreUsuarioPropietario=nombreUsu;
        }
        public static List<ListarTableroViewModel> FromTablero(List<Tablero> tableros)
        {
            List<ListarTableroViewModel> ListarTableroVM = new List<ListarTableroViewModel>();
            
            foreach (var tablero in tableros)
            {
                ListarTableroViewModel newTableroVM = new ListarTableroViewModel();
                newTableroVM.Id = tablero.Id;
                newTableroVM.Nombre = tablero.Nombre;
                newTableroVM.Descripcion = tablero.Descripcion;
                newTableroVM.EstadoTablero = (Proyecto.Models.EstadoTablero)tablero.EstadoTablero;
                newTableroVM.IdUsuarioPropietario = tablero.Propietario.Id;
                newTableroVM.NombreUsuarioPropietario = tablero.Propietario.Nombre;
                ListarTableroVM.Add(newTableroVM);
            }
            return(ListarTableroVM);
        }
    }
}