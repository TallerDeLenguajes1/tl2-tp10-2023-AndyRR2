using Proyecto.Models;

namespace Proyecto.Repositories{
    public interface ITareaRepository{
        public List<Tarea> GetAll();
        public Tarea GetById(int? idTarea);
        public void Create(Tarea newTarea);
        public void Update(Tarea newTarea);
        public void Remove(int? idTarea);
        public void Assign(int? idTarea, int? idUsuario);
        public void Disable(int? idTarea);
        public List<Tarea> GetByOwnerBoard(int? idTablero);
        public List<Tarea> GetByOwnerUser(int? idUsuario);
        public bool ChechAsignedTask(int? idUsuario, int? idTablero);
        public bool TaskExists(string? nombreTarea);
    }
}