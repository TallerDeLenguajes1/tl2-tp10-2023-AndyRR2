using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tp11.ViewModels;
using Tp11.Models;
/*public enum EstadoTarea{
  Ideas=1, 
  ToDo=2, 
  Doing=3, 
  Review=4, 
  Done=5
}*/
public class EditarTareaViewModel{
    private int? id;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id")]
    public int? Id { get => id; set => id = value; }
    private int? idTablero;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id Tablero")]
    public int? IdTablero { get => idTablero; set => idTablero = value; }
    private string? nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre Tarea")]
    public string? Nombre { get => nombre; set => nombre = value; }
    private EstadoTarea estado;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Estado")]
    public EstadoTarea Estado { get => estado; set => estado = value; }
    private string? descripcion;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Descripcion")]
    public string? Descripcion { get => descripcion; set => descripcion = value; }
    private string? color;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Color")]
    public string? Color { get => color; set => color = value; }
    private int? idUsuarioAsignado;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id Usuario Asignado")]
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }

    public static EditarTareaViewModel FromTarea(Tarea newTarea)
    {
        EditarTareaViewModel newTVM = new EditarTareaViewModel();
        newTVM.id = newTarea.Id;
        newTVM.idTablero = newTarea.IdTablero;
        newTVM.nombre = newTarea.Nombre;
        newTVM.estado = (Tp11.ViewModels.EstadoTarea)newTarea.Estado;
        newTVM.descripcion = newTarea.Descripcion;
        newTVM.color = newTarea.Color;
        newTVM.idUsuarioAsignado = newTarea.IdUsuarioAsignado;
        return(newTVM);
    }
}