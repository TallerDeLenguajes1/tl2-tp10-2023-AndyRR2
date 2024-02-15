namespace EspacioTareaRepository;

using Tp11.Models;

public interface ITareaRepository{
    public void Create(Tarea newTarea);
    public void Update(Tarea tarea);
    public Tarea GetById(int? Id);
    //public List<Tarea> GetTareasDeUsuario(int? Id);
    public List<Tarea> GetTareasDeUsuarioEnTablero(int? IdUsuario,int? IdTablero);
    //public List<Tarea> GetTareasDeTablero(int? Id);
    public void Remove(int? Id);
    public void AsignarUsuario(Tarea tareaModificada);
    public List<Tarea> GetAll();
    /*para mostrar todas las tareas de todos los usuarios en caso de se Admin*/
    public void InhabilitarDeUsuario(int? IdUsuario);
    public void InhabilitarDeTablero(int? IdUsuario);
    //public int ContarTareasEstado(int estado);
}