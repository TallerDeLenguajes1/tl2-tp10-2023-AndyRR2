using Proyecto.Models;

namespace Proyecto.Repositories{
    public interface ITareaRepository{
        public List<Tarea> GetAll();
        public Tarea GetById(int? idTarea);
        public void Create(Tarea newTarea);
        public void Update(Tarea newTarea);
        /*public void Remove(int? idTarea);
        public void Assign(int? idTarea, int? idUsuario);
        public void ChangeStatus(Tarea tarea);
        public void Disable(int? idTarea, int? idTablero);*/
        public List<Tarea> GetAllByOwnerBoard(int? idTablero);
        /*public List<Tarea> GetByOwnerUser(int? idUsuario);
        public bool TaskExists(string? nombreTarea);*/
    }
}