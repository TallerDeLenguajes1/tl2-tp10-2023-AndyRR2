using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tp11.ViewModels;
public class LoginViewModel{
    private string nombre;
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre de Usuario")]
    public string Nombre { get => nombre; set => nombre = value; }

    private string contrasenia;

    [Required(ErrorMessage = "Este campo es requerido.")]
    [PasswordPropertyText]
    [Display(Name = "Contraseña")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
}