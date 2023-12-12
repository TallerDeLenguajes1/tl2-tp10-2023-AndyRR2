namespace Tp11.Models;

using Tp11.ViewModels;

public enum EstadoTablero{
  Active=1,
  Unnactive=2
}
public class Tablero{
    private int? id;
    private int? idUsuarioPropietario;
    private string? nombre;
    private string? descripcion;  
    private EstadoTablero estado;

    public int? Id { get => id; set => id = value; }
    public int? IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string? Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }
    public EstadoTablero Estado { get => estado; set => estado = value; }

    public Tablero(){
        
    }
    public Tablero(int? Id, int? IdUsuarioPropietario, string? Nombre, string? Descripcion, EstadoTablero Estado){
        id=Id;
        idUsuarioPropietario = IdUsuarioPropietario;
        nombre=Nombre;
        descripcion=Descripcion;
        estado = Estado;
    }
    public static Tablero FromCrearTableroViewModel(CrearTableroViewModel tableroVM)
    {
        return new Tablero
        {
            id = tableroVM.Id,
            idUsuarioPropietario = tableroVM.IdUsuarioPropietario,
            nombre = tableroVM.Nombre,
            descripcion=tableroVM.Descripcion,
            estado = (Tp11.Models.EstadoTablero)tableroVM.Estado
        };
    }
    public static Tablero FromEditarTableroViewModel(EditarTableroViewModel tableroVM)
    {
        return new Tablero
        {
            id = tableroVM.Id,
            idUsuarioPropietario = tableroVM.IdUsuarioPropietario,
            nombre = tableroVM.Nombre,
            descripcion=tableroVM.Descripcion,
            estado = (Tp11.Models.EstadoTablero)tableroVM.Estado
        };
    }
}