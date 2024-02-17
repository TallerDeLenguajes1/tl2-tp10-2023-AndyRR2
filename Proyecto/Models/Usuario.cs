using Proyecto.ViewModels;

namespace Proyecto.Models{
    public class Usuario{
        public int? Id{get;set;}
        public string? Nombre{get;set;}
        public string? Contrasenia{get;set;}
        public NivelDeAcceso NivelDeAcceso{get;set;}
        public Usuario(){}
        public Usuario(int? id, string? nombre, string? contrasenia, NivelDeAcceso nivel){
            Id=id;
            Nombre=nombre;
            Contrasenia=contrasenia;
            NivelDeAcceso=nivel;
        }
        public static Usuario FromCrearUsuario(CrearUsuarioViewModel usuarioVM){
            return new Usuario
            {
                Nombre=usuarioVM.Nombre,
                Contrasenia=usuarioVM.Contrasenia,
                NivelDeAcceso=usuarioVM.NivelDeAcceso
            };
        }
        public static Usuario FromEditarUsuario(EditarUsuarioViewModel usuarioVM){
            return new Usuario
            {
                Id=usuarioVM.Id,
                Nombre=usuarioVM.Nombre,
                Contrasenia=usuarioVM.Contrasenia,
                NivelDeAcceso=usuarioVM.NivelDeAcceso
            };
        }
    }
}