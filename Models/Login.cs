using Tp11.ViewModels;

namespace Tp11.Models;

public enum NivelDeAcceso{
    simple = 1,
    admin = 2
}
public class Login{
    private NivelDeAcceso nivel;
    private string? nombre;
    private string contrasenia;
    public string? Nombre { get => nombre; set => nombre = value; }
    public NivelDeAcceso Nivel { get => nivel; set => nivel = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }

    public Login(){
        
    }
    public Login(LoginViewModel loginViewModel)
    {          
        Nombre = loginViewModel.Nombre;
        Contrasenia = loginViewModel.Contrasenia;
    }
}