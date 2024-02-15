using Proyecto.Models;

namespace Proyecto.Repositories{
    public interface ILoginRepository{
        public bool AutenticarUsuario(string nombreUsuario, string contraseña);
        public Usuario ObtenerUsuario(string nombreUsuario, string contraseña);
    }
}