namespace Tp11.ViewModels;

using System.ComponentModel.DataAnnotations;

public class CrearTareaViewModel{
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
    
}