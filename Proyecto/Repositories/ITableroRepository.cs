using Proyecto.Models;

namespace Proyecto.Repositories{
    public interface ITableroRepository{
        public List<Tablero> GetAll();
        public Tablero GetById(int? idTablero);
        public void Create(Tablero newTablero);
        public void Update(Tablero newTablero);
        public void Remove(int? idTablero);
        public void Disable(int? idTablero);
        public List<Tablero> GetByOwnerUser(int? idUsuario);
        public List<Tablero> GetByUserAsignedTask(int? idUsuario);
        public bool BoardExists(string? nombreTablero);
    }
}