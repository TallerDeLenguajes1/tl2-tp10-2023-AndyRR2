namespace Tp11.Models;

using Tp11.ViewModels;

public enum EstadoTarea{
  Ideas=1, 
  ToDo=2, 
  Doing=3, 
  Review=4, 
  Done=5,
  Unnactive=6
}
public class Tarea{
    private int? id;
    private int? idTablero;
    private string? nombre;
    private EstadoTarea estado;
    private string? descripcion;
    private string? color;
    private int? idUsuarioAsignado;
    private int? idUsuarioPropietario;
   
    public int? Id { get => id; set => id = value; }
    public int? IdTablero { get => idTablero; set => idTablero = value; }
    public string? Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }
    public string? Color { get => color; set => color = value; }
    public EstadoTarea Estado { get => estado; set => estado = value; }
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
    public int? IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }

    public Tarea(){

    }
    public Tarea(int? Id, int? IdTablero, string? Nombre, string? Descripcion, string? Color, EstadoTarea Estado, int? IdUsuarioA, int? IdUsuarioP){
      id=Id;
      idTablero=IdTablero;
      nombre=Nombre;
      descripcion=Descripcion;
      color=Color;
      estado=Estado;
      idUsuarioAsignado=IdUsuarioA;
      idUsuarioPropietario = IdUsuarioP;
    }
    public static Tarea FromTareaViewModel(TareaViewModel tareaVM)
    {
        return new Tarea
        {
            id = tareaVM.Id,
            idTablero = tareaVM.IdTablero,
            nombre = tareaVM.Nombre,
            descripcion = tareaVM.Descripcion,
            color = tareaVM.Color,
            estado = (Tp11.Models.EstadoTarea)tareaVM.Estado,
            idUsuarioAsignado = tareaVM.IdUsuarioAsignado,
            idUsuarioPropietario = tareaVM.IdUsuarioPropietario
        };
    }
}