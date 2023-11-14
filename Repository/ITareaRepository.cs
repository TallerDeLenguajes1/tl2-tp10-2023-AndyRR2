
using Tp10.Models;

namespace EspacioTareaRepository{
public interface ITareasRepository
{
    public Tarea Create(Tarea newTarea);
    public List<Tarea> GetAll();
    public Tarea GetById(int Id);
    public List<Tarea> GetTareasDeTablero(int Id);
    public List<Tarea> GetTareasDeUsuario(int Id);
    public void Remove(int Id);
    public void Update(Tarea tarea);
}
}