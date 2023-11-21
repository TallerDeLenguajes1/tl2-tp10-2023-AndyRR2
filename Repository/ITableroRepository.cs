namespace EspacioTableroRepository;
using Tp11.Models;

public interface ITableroRepository{
    public void Create(Tablero newTablero);
    public List<Tablero> GetAll();
    public Tablero GetById(int? Id);
    public void Remove(int? Id);
    public void Update(Tablero tablero);
    public List<Tablero> GetTablerosDeUsuario(int? idUsuario);
}