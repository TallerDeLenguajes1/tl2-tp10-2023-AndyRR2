namespace Tp11.ViewModels;

using System.ComponentModel.DataAnnotations;

using Tp11.Models;
public enum EstadoTablero{
  Active=1,
  Unnactive=2
}
public class TableroViewModel{
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
    private EstadoTablero estado;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Estado")]
    public EstadoTablero Estado { get => estado; set => estado = value; }

    public static TableroViewModel FromTablero(Tablero tablero)
    {
        return new TableroViewModel
        {
            id = tablero.Id,
            idUsuarioPropietario = tablero.IdUsuarioPropietario,
            nombre = tablero.Nombre,
            descripcion=tablero.Descripcion,
            estado = (Tp11.ViewModels.EstadoTablero)tablero.Estado
        };
    }
    public static List<TableroViewModel> FromTablero(List<Tablero> tableros)
    {
        List<TableroViewModel> ListarTableroVM = new List<TableroViewModel>();
        
            foreach (var tablero in tableros)
            {
                TableroViewModel newTVM = new TableroViewModel();
                newTVM.id = tablero.Id;
                newTVM.idUsuarioPropietario = tablero.IdUsuarioPropietario;
                newTVM.nombre = tablero.Nombre;
                newTVM.Descripcion = tablero.Descripcion;
                newTVM.estado = (Tp11.ViewModels.EstadoTablero)tablero.Estado;
                ListarTableroVM.Add(newTVM);
            }
            return(ListarTableroVM);
    }
}