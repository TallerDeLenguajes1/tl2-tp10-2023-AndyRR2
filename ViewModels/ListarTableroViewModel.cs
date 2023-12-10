namespace Tp11.ViewModels;

using System.ComponentModel.DataAnnotations;

using Tp11.Models;

public class ListarTableroViewModel{
    private int? id;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id")]
    public int? Id { get => id; set => id = value; }
    private int? idUsuarioPropietario;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id Usuario Propietario")]
    public int? IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    private string? nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre Tablero")]
    public string? Nombre { get => nombre; set => nombre = value; }
    private string? descripcion;  
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Descripcion")] 
    public string? Descripcion { get => descripcion; set => descripcion = value; }

    public static List<ListarTableroViewModel> FromTablero(List<Tablero> tableros)
    {
        List<ListarTableroViewModel> ListarTableroVM = new List<ListarTableroViewModel>();
        
            foreach (var tablero in tableros)
            {
                ListarTableroViewModel newTVM = new ListarTableroViewModel();
                newTVM.id = tablero.Id;
                newTVM.idUsuarioPropietario = tablero.IdUsuarioPropietario;
                newTVM.nombre = tablero.Nombre;
                newTVM.Descripcion = tablero.Descripcion;
                ListarTableroVM.Add(newTVM);
            }
            return(ListarTableroVM);
    }
}