using Tp11.Models;
namespace EspacioTareaRepository;

public interface ITareaRepository{
    public void Create(Tarea newTarea);
    public List<Tarea> GetAll();
    public Tarea GetById(int? Id);
    public void Remove(int? Id);
    public void Update(Tarea tarea);
    //public int ContarTareasEstado(int estado);
    public List<Tarea> GetTareasDeTablero(int? Id);
    public List<Tarea> GetTareasDeUsuario(int? Id);
    
}