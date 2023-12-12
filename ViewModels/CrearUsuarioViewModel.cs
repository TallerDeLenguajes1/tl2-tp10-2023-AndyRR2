namespace Tp11.ViewModels;

using System.ComponentModel.DataAnnotations;

using Tp11.Models;

public class CrearUsuarioViewModel{
    private int? id;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id")]
    public int? Id { get => id; set => id = value; }
    
    private string? nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nuevo nombre de Usuario")]
    public string? Nombre { get => nombre; set => nombre = value; }

    private string contrasenia;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nueva Contrasenia")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }

    private int nivel;
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Cambiar Nivel De Acceso")]
    public int Nivel { get => nivel; set => nivel = value; }

    public static CrearUsuarioViewModel FromUsuario(Usuario usuario)
    {
        return new CrearUsuarioViewModel
        {
            nombre = usuario.Nombre,
            id = usuario.Id,
            contrasenia = usuario.Contrasenia,
            nivel = usuario.Nivel
        };
    }
}