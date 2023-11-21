
using Tp11.Models;
using Tp11.ViewModels;
namespace EspacioUsuarioRepository;

public interface IUsuarioRepository{
    public List<ListarUsuarioViewModel> GetAll();
    public void Create(CrearUsuarioViewModel newUsuario);
    public Usuario GetById(int? idUsuario);
    public void Remove(int? idUsuario);
    public void Update(EditarUsuarioViewModel usuarioAEditar);
}