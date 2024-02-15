namespace Tp11.Models;

using Tp11.ViewModels;

public class Usuario{
    private int? id;
    private string? nombre;
    private string contrasenia;
    private int nivel;
    
    public int? Id { get => id; set => id = value; }
    public string? Nombre { get => nombre; set => nombre = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    public int Nivel { get => nivel; set => nivel = value; }

    public Usuario(){

    }
    public Usuario(int? Id, string? Nombre, string Contrasenia, int Nivel){
        id=Id;
        nombre=Nombre;
        contrasenia = Contrasenia;
        nivel = Nivel;
    }
    public static Usuario FromCrearUsuarioViewModel(CrearUsuarioViewModel usuarioVM)
    {
        return new Usuario
        {
            nombre = usuarioVM.Nombre,
            id = usuarioVM.Id,
            contrasenia = usuarioVM.Contrasenia,
            nivel = usuarioVM.Nivel
        };
    }
    public static Usuario FromEditarUsuarioViewModel(EditarUsuarioViewModel usuarioVM)
    {
        return new Usuario
        {
            nombre = usuarioVM.Nombre,
            id = usuarioVM.Id,
            contrasenia = usuarioVM.Contrasenia,
            nivel = usuarioVM.Nivel
        };
    }
}