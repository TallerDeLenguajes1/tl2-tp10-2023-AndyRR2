using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Tp11.ViewModels;
using Tp11.Models;

public class EditarTableroViewModel{
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

    public static EditarTableroViewModel FromTablero(Tablero tablero)
    {
        return new EditarTableroViewModel
        {
            id = tablero.Id,
            idUsuarioPropietario = tablero.IdUsuarioPropietario,
            nombre = tablero.Nombre,
            descripcion=tablero.Descripcion
        };
    }

}