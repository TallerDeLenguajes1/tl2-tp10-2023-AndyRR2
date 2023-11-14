using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tp10.Models;

namespace EspacioTableroRepository{
public interface ITablerosRepository
{
    public Tablero Create(Tablero newTablero);
    public List<Tablero> GetAll();
    public Tablero GetById(int Id);
    public void Remove(int Id);
    public void Update(Tablero usuario);
    public List<Tablero> GetListaTableros(int Id);
}
}