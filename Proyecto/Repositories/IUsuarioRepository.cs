using Proyecto.Models;

namespace Proyecto.Repositories{
    public interface IUsuarioRepository{
        public List<Usuario> GetAll();
        public Usuario GetById(int? idUsuario);
        public void Create(Usuario newUsuario);
        public void Update(Usuario newUsuario);
        public void Remove(int? idUsuario);
        public bool UserExists(string? nombreUsuario);
    }
}