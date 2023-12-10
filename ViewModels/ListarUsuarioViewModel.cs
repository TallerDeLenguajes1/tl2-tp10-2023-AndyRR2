namespace Tp11.ViewModels;

using System.ComponentModel.DataAnnotations;

using Tp11.Models;

public class ListarUsuarioViewModel{
    private int? id;

    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Id")]
    public int? Id { get => id; set => id = value; }
    
    private string? nombre;
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre")]
    public string? Nombre { get => nombre; set => nombre = value; }

    public static List<ListarUsuarioViewModel> FromUsuario(List<Usuario> usuarios)
    {
        List<ListarUsuarioViewModel> listaUsuariosVM = new List<ListarUsuarioViewModel>();
        
            foreach (var usuario in usuarios)
            {
                ListarUsuarioViewModel newUVM = new ListarUsuarioViewModel();
                newUVM.id = usuario.Id;
                newUVM.nombre = usuario.Nombre;
                listaUsuariosVM.Add(newUVM);
            }
            return(listaUsuariosVM);
    }
}