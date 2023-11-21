namespace Tp11.Models;

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

}