using Proyecto.Models;

namespace Proyecto.Repositories{
    public interface ITableroRepository{
        public List<Tablero> GetAll();
        public List<Tablero> GetAllByOwnerUser(int? idUsuario);
        public List<Tablero> GetAllByAsignedTask(int? idUsuario);
        public Tablero GetById(int? idTablero);
        public void Create(Tablero newTablero);
        public void Update(Tablero newTablero);
        public void Remove(int? idTablero);
        //public void Disable(int? idTablero);
        //public bool ChechAsignedTask(int? idUsuario, int? idTablero);
        public bool BoardExists(string? nombreTablero);
    }
}