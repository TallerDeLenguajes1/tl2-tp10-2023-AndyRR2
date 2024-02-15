namespace EspacioUsuarioRepository;

using Tp11.Models;

public interface IUsuarioRepository{
    public void Create(Usuario newUsuario);
    public void Update(Usuario usuarioAEditar);
    public Usuario GetById(int? idUsuario);
    public List<Usuario> GetAll();
    public void Remove(int? idUsuario);
}
