using Proyecto.Models;

namespace Proyecto.Repositories{
    public interface ITableroRepository{
        public List<Tablero> GetAll();
        public Tablero GetById(int? idTablero);
        public void Create(Tablero newTablero);
        public void Update(Tablero newTablero);
        public void Remove(int? idTablero);
        public List<Tablero> GetByOwnerUser(int? idUsuario);//Usado para obtener los tableros propiedad de un usuario para luego inhabilitarlos
        public void Disable(int? idTablero);//Usado para inhabilitar los tableros
        public List<Tablero> GetByUserAsignedTask(int? idUsuario);
    }
}