namespace Tp10.Models;

public class Usuario{
    private int id;
    private string? nombre;
    public int Id { get => id; set => id = value; }
    public string? Nombre { get => nombre; set => nombre = value; }

    public Usuario(){
        
    }
    public Usuario(int ID, string? NOMBRE){
        id=ID;
        nombre=NOMBRE;
    }
}