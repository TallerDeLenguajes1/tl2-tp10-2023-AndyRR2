using System.ComponentModel.DataAnnotations;

namespace Tp11.ViewModels;
using Tp11.Models;
public class EditarUsuarioViewModel{
    private int? id;

    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id")]
    public int? Id { get => id; set => id = value; }
    
    private string? nombre;
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nuevo nombre de Usuario")]
    public string? Nombre { get => nombre; set => nombre = value; }

    public static EditarUsuarioViewModel FromUsuario(Usuario usuario)
    {
        return new EditarUsuarioViewModel
        {
            nombre = usuario.Nombre,
            id = usuario.Id
        };
    }

}