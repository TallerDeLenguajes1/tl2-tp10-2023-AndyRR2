using System.ComponentModel.DataAnnotations;

using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class ListarTableroViewModel{

        [Display(Name = "Id")]
        public int? Id{get;set;}

        [Display(Name = "Id de Usuario Propietario")]
        public int? IdUsuarioPropietario{get;set;}

        [Display(Name = "Nombre Tablero")]
        public string? Nombre{get;set;}

        [Display(Name = "Descripción")]
        public string? Descripcion{get;set;}

        [Display(Name = "Estado")]
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