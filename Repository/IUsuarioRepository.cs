
using Tp11.Models;
using Tp11.ViewModels;
namespace EspacioUsuarioRepository;

public interface IUsuarioRepository{
    public List<Usuario> GetAll();
    public void Create(Usuario newUsuario);
    public Usuario GetById(int? idUsuario);
    public void Remove(int? idUsuario);
    public void Update(Usuario usuarioAEditar);
}