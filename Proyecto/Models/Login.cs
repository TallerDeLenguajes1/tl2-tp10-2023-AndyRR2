using Proyecto.ViewModels;

namespace Proyecto.Models{
    public enum NivelDeAcceso{
        admin=1,
        simple=2
    }
    public class Login{
        public string? Nombre{get;set;}
        public string? Contrasenia{get;set;}
        public NivelDeAcceso NivelDeAcceso{get;set;}
        public Login(){}
        public Login(string? nombre, string? contrasenia, NivelDeAcceso nivel){
            Nombre=nombre;
            Contrasenia=contrasenia;
            NivelDeAcceso=nivel;
        }

        public static Login FromLoginViewModel(LoginViewModel loginVM){
            return new Login
            {
                Nombre=loginVM.Nombre,
                Contrasenia=loginVM.Contrasenia
            };
        }
    }
}