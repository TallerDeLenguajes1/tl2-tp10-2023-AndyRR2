namespace Tp11.Models;

using Tp11.ViewModels;

public class Usuario{
    private int? id;
    private string? nombre;
    
    public int? Id { get => id; set => id = value; }
    public string? Nombre { get => nombre; set => nombre = value; }

    public Usuario(){

    }
    public Usuario(int? Id, string? Nombre){
        id=Id;
        nombre=Nombre;
    }
    public static Usuario FromCrearUsuarioViewModel(CrearUsuarioViewModel usuarioVM)
    {
        return new Usuario
        {
            nombre = usuarioVM.Nombre,
            id = usuarioVM.Id
        };
    }
    public static Usuario FromEditarUsuarioViewModel(EditarUsuarioViewModel usuarioVM)
    {
        return new Usuario
        {
            nombre = usuarioVM.Nombre,
            id = usuarioVM.Id
        };
    }

}