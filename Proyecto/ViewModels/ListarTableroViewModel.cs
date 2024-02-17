using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class ListarTableroViewModel{
        public int? Id{get;set;}
        public int? IdUsuarioPropietario{get;set;}
        public string? Nombre{get;set;}
        public string? Descripcion{get;set;}
        public EstadoTablero EstadoTablero{get;set;}
        public ListarTableroViewModel(){}
        public ListarTableroViewModel(int? id, int? idUsu, string? nombre, string? descripcion, EstadoTablero estado){
            Id=id;
            IdUsuarioPropietario=idUsu;
            Nombre=nombre;
            Descripcion=descripcion;
            EstadoTablero=estado;
        }
        public static List<ListarTableroViewModel> FromTablero(List<Tablero> tableros)
        {
            List<ListarTableroViewModel> ListarTableroVM = new List<ListarTableroViewModel>();
            
            foreach (var tablero in tableros)
            {
                ListarTableroViewModel newTableroVM = new ListarTableroViewModel();
                newTableroVM.Id = tablero.Id;
                newTableroVM.IdUsuarioPropietario = tablero.IdUsuarioPropietario;
                newTableroVM.Nombre = tablero.Nombre;
                newTableroVM.Descripcion = tablero.Descripcion;
                newTableroVM.EstadoTablero = (Proyecto.Models.EstadoTablero)tablero.EstadoTablero;
                ListarTableroVM.Add(newTableroVM);
            }
            return(ListarTableroVM);
        }
    }
}