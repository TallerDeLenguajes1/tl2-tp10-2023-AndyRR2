namespace EspacioTableroRepository;

using Tp11.Models;

public interface ITableroRepository{
    public void Create(Tablero newTablero);
    public void Update(Tablero tablero);
    public Tablero GetById(int? Id);
    public List<Tablero> GetAll();
    public List<Tablero> GetTablerosDeUsuario(int? idUsuario);
    public void Remove(int? Id);
}