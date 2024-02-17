using Proyecto.Models;

namespace Proyecto.ViewModels{
    public class ListarUsuarioViewModel{
        public int? Id{get;set;}
        public string? Nombre{get;set;}
        public NivelDeAcceso NivelDeAcceso{get;set;}
        public ListarUsuarioViewModel(){}
        public ListarUsuarioViewModel(int? id, string? nombre, NivelDeAcceso nivel){
            Id=id;
            Nombre=nombre;
            NivelDeAcceso=nivel;
        }
        public static List<ListarUsuarioViewModel> FromUsuario(List<Usuario> usuarios)
        {
            List<ListarUsuarioViewModel> listaUsuariosVM = new List<ListarUsuarioViewModel>();
            
            foreach (var usuario in usuarios)
            {
                ListarUsuarioViewModel usuarioVM = new ListarUsuarioViewModel();
                usuarioVM.Id = usuario.Id;
                usuarioVM.Nombre = usuario.Nombre;
                usuarioVM.NivelDeAcceso=usuario.NivelDeAcceso;
                listaUsuariosVM.Add(usuarioVM);
            }
            return(listaUsuariosVM);
        }
    }
}