using System.ComponentModel.DataAnnotations;

namespace Tp11.ViewModels;
public class CrearUsuarioViewModel{
    private int? id;

    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id")]
    public int? Id { get => id; set => id = value; }
    
    private string? nombre;
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre")]
    public string? Nombre { get => nombre; set => nombre = value; }
}